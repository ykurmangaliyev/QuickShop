using System.Threading.Tasks;

namespace QuickShop.PaymentProvider.Abstractions
{
    /// <summary>
    /// Interface for integration with 3rd party payment providers
    /// </summary>
    public interface IPaymentProvider
    {
        Task<string> CreateMerchant(string email);
    }
}
