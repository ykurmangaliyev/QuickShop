using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Logging
{
    public abstract class Logger : ILogger
    {
        public abstract void Log(LogLevel level, string message);

        public void Verbose(string message)
        {
            Log(LogLevel.Verbose, message);
        }

        public void Info(string message)
        {
            Log(LogLevel.Info, message);
        }

        public void Warning(string message)
        {
            Log(LogLevel.Warning, message);
        }

        public void Error(string message)
        {
            Log(LogLevel.Error, message);
        }
    }
}
