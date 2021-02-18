using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using QuickShop.PaymentProvider.Abstractions;
using QuickShop.PaymentProvider.Stripe.Configuration;
using Stripe;

namespace QuickShop.PaymentProvider.Stripe
{
    public class StripePaymentProvider : IPaymentProvider
    {
        private readonly StripeOptions _options;

        public StripePaymentProvider(IOptions<StripeOptions> options)
        {
            _options = options.Value;
        }

        private IStripeClient CreateClient()
        {
            return new StripeClient(_options.ApiKey);
        }

        public async Task<string> CreateMerchant(string email)
        {
            var client = CreateClient();
            var service = new AccountService(client);

            var options = new AccountCreateOptions
            {
                Type = "standard",
                Email = email,
            };

            var account = await service.CreateAsync(options);

            return account.Id;
        }
    }
}
