using System;
using System.Reflection;

namespace API.Model
{
    public class PingResponse
    {
        public string Version { get; set; } 

        public DateTimeOffset ServerTime { get; set; }

        public static PingResponse Create()
        {
            return new PingResponse
            {
                Version = Assembly.GetExecutingAssembly().GetName().Version.ToString(),
                ServerTime = DateTimeOffset.Now,
            };
        }
    }
}