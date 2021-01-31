using System;

namespace QuickShop.Domain.Ping.Model
{
    public class PingCheck
    {
        public DateTimeOffset ServerTime { get; set; }

        public bool DatabaseStatus { get; set; }

        public double? DatabasePing { get; set; }
    }
}
