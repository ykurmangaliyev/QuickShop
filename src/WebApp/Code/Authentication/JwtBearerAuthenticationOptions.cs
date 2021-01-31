namespace QuickShop.WebApp.Authentication
{
    public class JwtBearerAuthenticationOptions
    {
        public const string JwtBearerAuthentication = nameof(JwtBearerAuthentication);

        public const int DefaultExpirationTimeInSeconds = 24 * 60 * 60; // 1 day

        public string Audience { get; set; }

        public string Authority { get; set; }

        public string SymmetricKey { get; set; }

        public int ExpirationTimeInSeconds { get; set; } = DefaultExpirationTimeInSeconds;
    }
}