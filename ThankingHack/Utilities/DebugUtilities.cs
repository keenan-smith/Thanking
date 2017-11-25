using System;
using System.IO;
using UnityEngine;

namespace Thanking.Utilities
{
    public class DebugUtilities
    {
        public static String DebugPath = $"{Application.dataPath}/Thanking.log";
        
        public static void Log(object Output) => 
            File.AppendAllText(DebugPath, $"{Output}\r\n");
        
        public static void LogException(Exception Exception) =>
            File.AppendAllText(DebugPath, $"\r\nBEGIN EXCEPTION\r\n{Exception}\r\nEND EXCEPTION\r\n");

        public static void Init() =>
            File.WriteAllText(DebugPath, $"{DateTime.Now}\r\n\r\n");
    }
}