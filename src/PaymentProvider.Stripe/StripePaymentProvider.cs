using System.Threading.Tasks;
using QuickShop.PaymentProvider.Abstractions;
using QuickShop.PaymentProvider.Stripe.Configuration;

namespace QuickShop.PaymentProvider.Stripe
{
    public class StripePaymentProvider : IPaymentProvider
    {
        private readonly StripeOptions _options;

        public StripePaymentProvider(StripeOptions options)
        {
            _options = options;
        }

        public Task<string> CreateMerchant()
        {
            throw new System.NotImplementedException();
        }
    }
}
