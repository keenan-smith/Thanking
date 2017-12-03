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
	    // Where the config file is located
	    public static String ConfigPath = $"{Application.dataPath}/Thanking.config";

	    // Version of the config file
	    public static String ConfigVersion = "1.0.1";
	    
	    // Load the config
		public static void Init()
		{
			#if DEBUG
			DebugUtilities.Log("Getting Thanking configuration...");
			#endif
			
			LoadConfig(GetConfig());
		}

	    // Collect all variables marked to be saved
	    public static Dictionary<String, object> CollectConfig()
	    {
		    // Create dictionary for variable name and value
		    // Also add the version variable first
		    Dictionary<String, object> ConfigFields = new Dictionary<string, object> {{"Version", ConfigVersion}};

		    // Get all classes in assembly
		    Type[] Types = Assembly.GetExecutingAssembly().GetTypes().Where(T => T.IsClass).ToArray();

		    // Loop through all classes
		    for (int i = 0; i < Types.Length; i++)
		    {
			    Type Type = Types[i];
			    
			    // Get all fields in class marked with SaveAttribute
			    FieldInfo[] Fields = Type.GetFields().Where(F => F.IsDefined(typeof(SaveAttribute), false)).ToArray();

			    // Loop through all marked fields
			    for (int o = 0; o < Fields.Length; o++)
			    {
				    FieldInfo Field = Fields[o];
				    
				    // Add the field to the collection
				    ConfigFields.Add(Type.Name + "_" + Field.Name, Field.GetValue(null));
			    }
		    }

		    return ConfigFields;
	    }

	    /// <summary>
	    /// Get Config if it exists. If not, create one
	    /// </summary>
	    /// <returns>Dictionary of variable names and values</returns>
	    public static Dictionary<String, object> GetConfig()
		{
			// Create config if it doesn't exist
			if (!File.Exists(ConfigPath))
				SaveConfig(CollectConfig());

			// Read and return the config
			return JsonConvert.DeserializeObject<Dictionary<String, object>>(File.ReadAllText(ConfigPath),
				new JsonSerializerSettings {Formatting = Formatting.Indented});
		}

	    /// <summary>
	    /// Save the configuration file in indented JSON
	    /// </summary>
	    /// <param name="Config">Dictionary of variable names and values</param>
	    public static void SaveConfig(Dictionary<String, object> Config) =>
		    File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Config, Formatting.Indented));

	    /// <summary>
	    /// Assign all variables to their configured values
	    /// </summary>
	    /// <param name="Config">Dictionary of variable names and values</param>
	    public static void LoadConfig(Dictionary<String, object> Config)
        {
	        // Update config if there is a version mismatch
	        if (!Config.ContainsKey("Version") || (String) Config["Version"] != ConfigVersion)
	        {
		        #if DEBUG
		        DebugUtilities.Log("Config version mismatch, updating");
		        #endif
		        
		        File.Delete(ConfigPath);
		        Init();
		        return;
	        }
	        
	        // Collect all classes in assembly
            Type[] Types = Assembly.GetExecutingAssembly().GetTypes().Where(T => T.IsClass).ToArray();

	        // Loop through all classes
	        for (int i = 0; i < Types.Length; i++)
	        {
		        Type Type = Types[i];
		        
		        // Get all fields marked with SaveAttribute
		        FieldInfo[] Fields = Type.GetFields().Where(F => F.IsDefined(typeof(SaveAttribute), false)).ToArray();

		        // Loop through all marked fields
		        for (int o = 0; o < Fields.Length; o++)
		        {
			        FieldInfo Field = Fields[o];

			        String Name = $"{Type.Name}_{Field.Name}";

			        // If Field is not in config, skip this iteration
			        if (!Config.ContainsKey(Name)) continue;
			        
			        // If field is an array, set the field variable to a JArray
			        if (Config[Name].GetType() == typeof(JArray))
				        Config[Name] = ((JArray) Config[Name]).ToObject(Field.FieldType);

			        // If field is an object, set the field variable to JObject
			        if (Config[Name].GetType() == typeof(JObject))
				        Config[Name] = ((JObject) Config[Name]).ToObject(Field.FieldType);

			        // Assign field values
			        Field.SetValue(null,
				        Field.FieldType.IsEnum
					        ? Enum.ToObject(Field.FieldType, Config[Name])
					        : Convert.ChangeType(Config[Name], Field.FieldType));
		        }
	        }
        }
    }
}