using System;
using System.IO;
using UnityEngine;

namespace Thanking.Utilities
{
    public class DebugUtilities
    {
        public static String DebugPath = $"{Application.dataPath}/Thanking.log";
        
        public static void Log(object Output) =>
             Debug.Log($"{Output}\r\n");
        
        public static void LogException(Exception Exception) =>
            Debug.Log($"\r\nBEGIN EXCEPTION\r\n{Exception}\r\nEND EXCEPTION\r\n");

        public static void Init() =>
            Debug.Log($"{DateTime.Now}\r\n\r\n");
    }
}