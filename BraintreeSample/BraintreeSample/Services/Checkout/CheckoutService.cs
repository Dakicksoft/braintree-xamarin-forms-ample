using BraintreeSample.Services.Base;
using BraintreeSample.Services.RequestProvider;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BraintreeSample.Services.Checkout
{
  public class CheckoutService : BaseService, ICheckoutService
  {
    readonly IRequestProvider _requestProvider;

    public CheckoutService(IRequestProvider requestProvider)
    {
      _requestProvider = requestProvider;
    }

    public async Task<Dictionary<string, string>> GetClientTokenAsync(string customerId)
    {
      Dictionary<string, string> result =new Dictionary<string, string>();

      if (CrossConnectivity.Current.IsConnected)
      {
        var builder = new UriBuilder(GlobalSettings.BASE_SERVER_URL)
        {
          Path = $"api/checkouts/ClientToken",
          Query = $"customerId={customerId}"
        };
        var uri = builder.ToString();

        result = await _requestProvider.GetAsync<Dictionary<string, string>>(uri);
      }
      return result;
    }

    public async Task<bool> MakeTransctionAsync(decimal amount,string nonce,string customerId, string merchantAccountId = null, bool? threeDSecureRequired = null)
    {
      bool result = false;

      if (CrossConnectivity.Current.IsConnected)
      {
        var builder = new UriBuilder(GlobalSettings.BASE_SERVER_URL)
        {
          Path = $"api/checkouts/MakeTransction",
          Query = $"amount={amount}&nonce={nonce}&customerId={customerId}&merchantAccountId={merchantAccountId}"
        };
        var uri = builder.ToString();

        result = await _requestProvider.GetAsync<bool>(uri);
      }
      return result;
    }
  }
}
