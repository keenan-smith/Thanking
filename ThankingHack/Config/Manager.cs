using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;

namespace Thanking.Config
{
    public class Manager
    {
        public static Dictionary<String, object> CollectConfig()
        {
            Dictionary<String, object> ConfigFields = new Dictionary<string, object>();
            
            Type[] Types = Assembly.GetExecutingAssembly().GetTypes();

            for (int i = 0; i < Types.Length; i++)
            {
                Type Type = Types[i];
                FieldInfo[] Fields = Type.GetFields();

                for (int o = 0; o < Fields.Length; o++)
                {
                    FieldInfo Field = Fields[o];
                    
                    if(Field.IsDefined(typeof(Save), false))
                        ConfigFields.Add(Field.Name, Field.GetValue(null));
                }
            }

            return ConfigFields;
        }

        public static void SaveConfig(Dictionary<String, object> Config)
        {
            String Path = $"{Application.dataPath}/Thanking.config";

            File.WriteAllText(Path, JsonUtility.ToJson(Config));
        }

        public static void LoadConfig(Dictionary<String, object> Config)
        {
            Type[] Types = Assembly.GetExecutingAssembly().GetTypes();

            for (int i = 0; i < Types.Length; i++)
            {
                Type Type = Types[i];
                FieldInfo[] Fields = Type.GetFields();

                for (int o = 0; o < Fields.Length; o++)
                {
                    FieldInfo Field = Fields[o];
                    
                    if(Field.IsDefined(typeof(Save), false) && Config.ContainsKey(Field.Name))
                        Field.SetValue(null, Config[Field.Name]);
                }
            }
        }
    }
}