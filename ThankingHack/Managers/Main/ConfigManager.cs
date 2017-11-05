using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Thanking.Attributes;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Thanking.Managers.Main
{
    public class ConfigManager
    {
		public static void Init()
		{
			Debug.Log("Getting Thanking configuration...");
			LoadConfig(GetConfig());
		}

	    public static Dictionary<String, object> CollectConfig()
	    {
		    Dictionary<String, object> ConfigFields = new Dictionary<string, object>();

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
			String Path = $"{Application.dataPath}/Thanking.config";

			if (!File.Exists(Path))
				SaveConfig(CollectConfig());

			return JsonConvert.DeserializeObject<Dictionary<String, object>>(File.ReadAllText(Path), new JsonSerializerSettings() { Formatting = Formatting.Indented });
		}

		public static void SaveConfig(Dictionary<String, object> Config)
        {
            String Path = $"{Application.dataPath}/Thanking.config";

            File.WriteAllText(Path, JsonConvert.SerializeObject(Config, Formatting.Indented));
        }

        public static void LoadConfig(Dictionary<String, object> Config)
        {
            Type[] Types = Assembly.GetExecutingAssembly().GetTypes().Where(T => T.IsClass).ToArray();

	        for (int i = 0; i < Types.Length; i++)
	        {
		        Type Type = Types[i];
		        FieldInfo[] Fields = Type.GetFields().Where(F => F.IsDefined(typeof(SaveAttribute), false)).ToArray();

		        for (int o = 0; o < Fields.Length; o++)
		        {
			        FieldInfo Field = Fields[o];

			        string Name = Type.Name + "_" + Field.Name;

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