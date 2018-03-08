using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraintreeSample.Services.Checkout
{
  public interface ICheckoutService
  {
    Task<Dictionary<string, string>> GetClientTokenAsync(string customerId);

    Task<bool> MakeTransctionAsync(decimal amount, string nonce,string customerId, string merchantAccountId = null, bool? threeDSecureRequired = null);
  }
}
