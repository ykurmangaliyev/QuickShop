using System;
using System.Collections.Generic;

namespace Logging
{
    public class ConsoleLogger : Logger
    {
        public override void Log(LogLevel level, string message)
        {
            Console.WriteLine($"[{DateTime.Now:s}] [{level.ToString("G").ToUpper()}] {message}");
        }
    }
}