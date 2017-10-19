using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Thanking.Attributes;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Thanking.Managers
{
    public class ConfigManager
    {
		public static void Init() =>
			LoadConfig(GetConfig());

        public static Dictionary<String, object> CollectConfig()
        {
            Dictionary<String, object> ConfigFields = new Dictionary<string, object>();
            
            Type[] Types = Assembly.GetExecutingAssembly().GetTypes();

            for (int i = 0; i < Types.Length; i++)
            {
                Type Type = Types[i];
				
				if (Type.IsClass)
				{
					FieldInfo[] Fields = Type.GetFields();

					for (int o = 0; o < Fields.Length; o++)
					{
						FieldInfo Field = Fields[o];

						if (Field.IsDefined(typeof(SaveAttribute), false))
							ConfigFields.Add(Type.Name + "_" + Field.Name, Field.GetValue(null));
					}
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
            Type[] Types = Assembly.GetExecutingAssembly().GetTypes();

            for (int i = 0; i < Types.Length; i++)
            {
                Type Type = Types[i];

				if (Type.IsClass)
				{
					FieldInfo[] Fields = Type.GetFields();

					for (int o = 0; o < Fields.Length; o++)
					{
						FieldInfo Field = Fields[o];

						string Name = Type.Name + "_" + Field.Name;

						if (Field.IsDefined(typeof(SaveAttribute), false) && Config.ContainsKey(Name))
						{
							if (Config[Name].GetType() == typeof(JArray))
								Config[Name] = ((JArray)Config[Name]).ToObject(Field.FieldType);

							if (Config[Name].GetType() == typeof(JObject))
								Config[Name] = ((JObject)Config[Name]).ToObject(Field.FieldType);

							if (Field.FieldType.IsEnum)
								Field.SetValue(null, Enum.ToObject(Field.FieldType, Config[Name]));
							else
								Field.SetValue(null, Convert.ChangeType(Config[Name], Field.FieldType));
						}
					}
				}
            }
        }
    }
}