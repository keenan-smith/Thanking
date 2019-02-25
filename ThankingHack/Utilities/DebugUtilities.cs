using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;
using Thanking.Attributes;

namespace Thanking.Utilities
{
    public class DebugUtilities
    {
        public static ConcurrentQueue<string> Data = new ConcurrentQueue<string>();

        [Thread]
        public static void DebugThread()
        {
            File.WriteAllText("th.log", "");
            Data.Enqueue($"Thanking Debug Init Start: {DateTime.Now}\r\n\r\n");

            while (true)
            {
                Thread.Sleep(500);
                while (Data.Count > 0)
                    if (Data.TryDequeue(out string str))
                        File.AppendAllText("th.log", str);
            }
        }
        
        public static void Log(object Output) =>
            Data.Enqueue($"{Output}\r\n");
        
        public static void LogException(Exception Exception) =>
            Data.Enqueue($"\r\nBEGIN EXCEPTION\r\n{Exception}\r\nEND EXCEPTION\r\n");
    }
}
