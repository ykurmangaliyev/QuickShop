using System;

namespace QuickShop.Domain.Accounts.Model.UserAggregate
{
    public class EmailDetails
    {
        public bool IsVerified { get; private set; } = false;

        public string Email { get; private set; } = String.Empty;
    }
}