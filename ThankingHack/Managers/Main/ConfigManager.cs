﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Thanking.Attributes;
using Thanking.Utilities;
using UnityEngine;
using System.Collections;
using System.ComponentModel;
using System.Text.RegularExpressions;
using Newtonsoft.Json.Schema;

namespace Thanking.Managers.Main
{
    public class ConfigManager
    {
	    // Where the config file is located
	    public static string ConfigPath = $"{Application.dataPath}/Thanking.config";

	    // Version of the config file
	    public static string ConfigVersion = "1.0.1";
	    
	    // Load the config
		public static void Init()
		{
			#if DEBUG
			DebugUtilities.Log("Getting Thanking configuration...");
			#endif
			
			LoadConfig(GetConfig());
		}

	    // Collect all variables marked to be saved
	    public static Dictionary<string, object> CollectConfig()
	    {
		    // Create dictionary for variable name and value
		    // Also add the version variable first
		    Dictionary<string, object> ConfigFields = new Dictionary<string, object> {{"Version", ConfigVersion}};

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
	    public static Dictionary<string, object> GetConfig()
		{
			// Create config if it doesn't exist
			if (!File.Exists(ConfigPath))
				SaveConfig(CollectConfig());

			// Read and return the config
			return JsonConvert.DeserializeObject<Dictionary<string, object>>(File.ReadAllText(ConfigPath),
				new JsonSerializerSettings {Formatting = Formatting.Indented});
		}

	    /// <summary>
	    /// Save the configuration file in indented JSON
	    /// </summary>
	    /// <param name="Config">Dictionary of variable names and values</param>
	    public static void SaveConfig(Dictionary<string, object> Config) =>
		    File.WriteAllText(ConfigPath, JsonConvert.SerializeObject(Config, Formatting.Indented));

	    /// <summary>
	    /// Assign all variables to their configured values
	    /// </summary>
	    /// <param name="Config">Dictionary of variable names and values</param>
	    public static void LoadConfig(Dictionary<string, object> Config)
        {
			foreach (Type AssemblyType in Assembly.GetExecutingAssembly().GetTypes()) // Loop through all types in the assembly
			{
				foreach (FieldInfo FInfo in AssemblyType.GetFields().Where(f => Attribute.IsDefined(f, typeof(SaveAttribute)))) // Loop through all fields with the save attribute defined
				{
					// Get field name and type in our format
					string Name = $"{AssemblyType.Name}_{FInfo.Name}";
					Type FIType = FInfo.FieldType;

					object DefaultInfo = FInfo.GetValue(null); // Get the default value for the fieldinfo

					
					if (!Config.ContainsKey(Name)) // If the field does not exist in the configuration dictionary
						Config.Add(Name, DefaultInfo);
					
					try //Try to parse intermediate JSON because the object itself isnt actually deserialized or something
					{
						// If field is an array, set the field variable to a JArray
						if (Config[Name].GetType() == typeof(JArray))
							Config[Name] = ((JArray) Config[Name]).ToObject(FInfo.FieldType);

						// If field is an object, set the field variable to JObject
						if (Config[Name].GetType() == typeof(JObject))
							Config[Name] = ((JObject) Config[Name]).ToObject(FInfo.FieldType);

						// Assign field values
						FInfo.SetValue(null,
							FInfo.FieldType.IsEnum
								? Enum.ToObject(FInfo.FieldType, Config[Name])
								: Convert.ChangeType(Config[Name], FInfo.FieldType));
					}
					catch
					{
						Config[Name] = DefaultInfo;
					}
					SaveConfig(Config);
				}
			}
        }
    }
}