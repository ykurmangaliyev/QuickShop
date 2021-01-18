using System.Collections.Generic;

namespace Logging
{
    public interface ILogger
    {
        void Log(LogLevel level, string message);

        void Verbose(string message);

        void Info(string message);

        void Warning(string message);

        void Error(string message);
    }
}