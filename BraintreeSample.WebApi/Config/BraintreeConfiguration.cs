using Braintree;
using Microsoft.Extensions.Configuration;

namespace BraintreeSample.WebApi.Config
{
  public class BraintreeConfiguration : IBraintreeConfiguration
  {
    public string Environment { get; set; }
    public string MerchantId { get; set; }
    public string PublicKey { get; set; }
    public string PrivateKey { get; set; }
    private IBraintreeGateway BraintreeGateway { get; set; }

    public readonly IConfiguration Configuration;

    public BraintreeConfiguration(IConfiguration configuration)
    {
      Configuration = configuration;
    }
    public IBraintreeGateway CreateGateway()
    {
      Environment = Configuration["Braintree:Environment"];
      MerchantId = Configuration["Braintree:MerchantId"];
      PublicKey = Configuration["Braintree:PublicKey"];
      PrivateKey = Configuration["Braintree:PrivateKey"];

      if (MerchantId == null || PublicKey == null || PrivateKey == null)
      {
        Environment = GetConfigurationSetting("BraintreeEnvironment");
        MerchantId = GetConfigurationSetting("BraintreeMerchantId");
        PublicKey = GetConfigurationSetting("BraintreePublicKey");
        PrivateKey = GetConfigurationSetting("BraintreePrivateKey");
      }

      return new BraintreeGateway(Environment, MerchantId, PublicKey, PrivateKey);
    }

    public string GetConfigurationSetting(string setting)
    {
      return Configuration[setting];
    }

    public IBraintreeGateway GetGateway()
    {
      if (BraintreeGateway == null)
      {
        BraintreeGateway = CreateGateway();
      }

      return BraintreeGateway;
    }
  }
}
