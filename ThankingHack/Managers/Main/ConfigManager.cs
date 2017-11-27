using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Thanking.Attributes;
using Thanking.Utilities;
using UnityEngine;

namespace Thanking.Managers.Main
{
    public class ConfigManager
    {
	    public static String ConfigPath = $"{Application.dataPath}/Thanking.config";

	    public static String ConfigVersion = "1.0.1";
	    
		public static void Init()
		{
			#if DEBUG
			DebugUtilities.Log("Getting Thanking configuration...");
			#endif
			
			LoadConfig(GetConfig());
		}

	    public static Dictionary<String, object> CollectConfig()
	    {
		    Dictionary<String, object> ConfigFields = new Dictionary<string, object>();
		    
		    ConfigFields.Add("Version", ConfigVersion);

		    Type[] Types = Assembly.GetExecutingAssembly().GetTypes().Where(T => T.IsClass).ToArray();

		    for (int i = 0; i < Types.Length; i++)
		    {
			    Type Type = Types[i];
			    FieldInfo[] Fields = Type.GetFields().Where(F => F.IsDefined(typeof(SaveAttribute), false)).ToArray();

			    for (int o = 0; o < Fields.Length; o++)
			    {
				    FieldInfo Field = Fields[o];
				    ConfigFields.Add(Type.Name + "_" + Field.Name, Field.GetValue(null));
			    }
		    }

		    return ConfigFields;
	    }

	    public static Dictionary<String, object> GetConfig()
		{
			if (!File.Exists(ConfigPath))
				SaveConfig(CollectConfig());

			return JsonConvert.DeserializeObject<Dictionary<String, object>>(File.ReadAllText(ConfigPath),
				new JsonSerializerSettings {Formatting = Formatting.Indented});
		}

	    public static void SaveConfig(Dictionary<String, object> Config) =>
		    File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Config, Formatting.Indented));

	    public static void LoadConfig(Dictionary<String, object> Config)
        {
	        if (!Config.ContainsKey("Version") || (String) Config["Version"] != ConfigVersion)
	        {
		        #if DEBUG
		        DebugUtilities.Log("Config version mismatch, updating");
		        #endif
		        
		        File.Delete(ConfigPath);
		        Init();
		        return;
	        }
	        
            Type[] Types = Assembly.GetExecutingAssembly().GetTypes().Where(T => T.IsClass).ToArray();

	        for (int i = 0; i < Types.Length; i++)
	        {
		        Type Type = Types[i];
		        FieldInfo[] Fields = Type.GetFields().Where(F => F.IsDefined(typeof(SaveAttribute), false)).ToArray();

		        for (int o = 0; o < Fields.Length; o++)
		        {
			        FieldInfo Field = Fields[o];

			        String Name = $"{Type.Name}_{Field.Name}";

			        if (!Config.ContainsKey(Name)) continue;
			        
			        if (Config[Name].GetType() == typeof(JArray))
				        Config[Name] = ((JArray) Config[Name]).ToObject(Field.FieldType);

			        if (Config[Name].GetType() == typeof(JObject))
				        Config[Name] = ((JObject) Config[Name]).ToObject(Field.FieldType);

			        Field.SetValue(null,
				        Field.FieldType.IsEnum
					        ? Enum.ToObject(Field.FieldType, Config[Name])
					        : Convert.ChangeType(Config[Name], Field.FieldType));
		        }
	        }
        }
    }
}