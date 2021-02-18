using System;

namespace QuickShop.Domain.Accounts.Model.UserAggregate
{
    public class StripeDetails
    {
        public bool IsVerified { get; private set; } = false;

        public string Merchant { get; private set; } = String.Empty;
    }
}