using System;
using System.IO;
using UnityEngine;

namespace Thanking.Utilities
{
    public class DebugUtilities
    {
        public static String DebugPath = $"{Application.dataPath}/Thanking.log";
        
        public static void Log(String Output) => 
            File.AppendAllText(DebugPath, $"{Output}\r\n");

        public static void Init() =>
            File.WriteAllText(DebugPath, $"{DateTime.Now.ToString()}\r\n");
    }
}