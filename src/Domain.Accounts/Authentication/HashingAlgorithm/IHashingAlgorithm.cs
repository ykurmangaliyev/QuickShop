namespace QuickShop.Domain.Accounts.Authentication.HashingAlgorithm
{
    public interface IHashingAlgorithm
    {
        string Hash(string digest);
    }
}
