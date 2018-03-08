using System.Threading.Tasks;

namespace BraintreeSample.Services
{
  public interface IDropInService
  {
    Task<DropInResult> ShowDropInAsync(DropInRequest request);

    bool IsValidToken(string clientToken);
  }

  public class DropInRequest
  {
    public string Authorization { get; set; }

    public string Amount { get; set; }

    public string CurrencyCode { get; set; }

    public bool? ThreeDSecureVerification { get; set; }

    public bool ApplePayDisabled { get; set; }

    public bool PaypalDisabled { get; set; }

    public bool VenmoDisabled { get; set; }

    public bool AndroidPayDisabled { get; set; }

    public bool GooglePaymentDisabled { get; set; }

    public bool ShouldMaskSecurityCode { get; set; }

    public bool ShouldCollectDeviceData { get; set; }

    public DropInTheme Theme { get; set; }
  }

  public class DropInResult
  {
    public PaymentOptionType OptionType { get; set; }

    public string Description { get; set; }

    public PaymentMethod Method { get; set; }

  }

  public enum PaymentOptionType
  {
    Unknown = 0,
    AMEX,
    DinersClub,
    Discover,
    MasterCard,
    Visa,
    JCB,
    Laser,
    Maestro,
    UnionPay,
    Solo,
    Switch,
    UKMaestro,
    PayPal,
    Coinbase,
    Venmo,
    ApplePay,

    AndroidPay = 9999
  }

  public enum DropInTheme
  {
    Default,
    Dark,
    Light
  }

  public class PaymentMethod
  {
    public string Nonce { get; set; }

    public string LocalizedDescription { get; set; }

    public string Type { get; set; }

    public bool IsDefault { get; set; }
  }
}
