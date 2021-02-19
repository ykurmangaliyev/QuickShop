using System;

namespace QuickShop.WebApp.Model
{
    public class PingResponse
    {
        public DateTimeOffset ServerTime { get; set; }

        public bool DatabaseStatus { get; set; }

        public double? DatabasePing { get; set; }
    }
}