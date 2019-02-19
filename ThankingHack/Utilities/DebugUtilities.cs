using System;
using System.IO;
using UnityEngine;

namespace Thinking.Utilities
{
    public class DebugUtilities
    {
        public static void Log(object Output) =>
            File.AppendAllText("th.log", $"{Output}\r\n");
        
        public static void LogException(Exception Exception) =>
            File.AppendAllText("th.log", $"\r\nBEGIN EXCEPTION\r\n{Exception}\r\nEND EXCEPTION\r\n");

        public static void Init() =>
            File.AppendAllText("th.log", $"Thanking Debug Init Start: {DateTime.Now}\r\n\r\n");
    }
}
