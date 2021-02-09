using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuickShop.PaymentProvider.Stripe.Configuration
{
    public class StripeOptions
    {
        public const string Stripe = nameof(Stripe);

        public string ApiKey { get; set; }
    }
}
