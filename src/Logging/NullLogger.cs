namespace Logging
{
    public class NullLogger : Logger
    {
        public override void Log(LogLevel level, string message)
        {
            // Intentionally do nothing
        }
    }
}