using System;
using System.Threading.Tasks;
using Xamarin.Forms;
using Android.App;
using Com.Braintreepayments.Api.Interfaces;
using Com.Braintreepayments.Api.Models;
using Android.Content;
using Newtonsoft.Json;
using Android.Gms.Wallet;
using Com.Braintreepayments.Api;
using Com.Braintreepayments.Api.Dropin;
using Com.Braintreepayments.Api.Dropin.Utils;
using BraintreeSample.Droid;

//[assembly: Dependency(typeof(DropInService))]

namespace BraintreeSample.Droid
{
  public class DropInService : Services.IDropInService
  {
    public bool IsValidToken(string clientToken)
    {
      return ClientToken.FromString(clientToken) is ClientToken;
    }

    public Task<Services.DropInResult> ShowDropInAsync(Services.DropInRequest request)
    {
      var tcs = new TaskCompletionSource<Services.DropInResult>();

      BraintreeDropInHandlerActivity.ShowDropIn(tcs, request);

      return tcs.Task;
    }
  }

  [Activity(Label = "Braintree Handler", Theme = "@android:style/Theme.NoDisplay")]
  class BraintreeDropInHandlerActivity :
      Activity,
      IPaymentMethodNonceCreatedListener,
      IBraintreeCancelListener,
      IBraintreeErrorListener,
      DropInResult.IDropInResultListener
  {
    static string DROP_IN_REQUEST_KEY = "DROP_IN_REQUEST_KEY";
    static int DROP_IN_REQUEST = 1001;

    protected override void OnCreate(Android.OS.Bundle savedInstanceState)
    {
      base.OnCreate(savedInstanceState);

      var request = JsonConvert.DeserializeObject<Services.DropInRequest>(Intent.GetStringExtra(DROP_IN_REQUEST_KEY));

      //TODO Handle AndroidPay/GooglePayment
      var dropInRequest = new DropInRequest()
          .ClientToken(request.Authorization)
          .Amount(request.Amount)
          .RequestThreeDSecureVerification(request.ThreeDSecureVerification == true)
          .CollectDeviceData(request.ShouldCollectDeviceData)
          //.GooglePaymentRequest(GetGooglePaymentRequest(request))
          //.AndroidPayCart(GetAndroidPayCart())
          //.AndroidPayShippingAddressRequired(Settings.isAndroidPayShippingAddressRequired(this))
          //.AndroidPayPhoneNumberRequired(Settings.isAndroidPayPhoneNumberRequired(this))
          //.AndroidPayAllowedCountriesForShipping(Settings.getAndroidPayAllowedCountriesForShipping(this))
          ;

      if (request.VenmoDisabled)
      {
        dropInRequest.DisableVenmo();
      }

      if (request.PaypalDisabled)
      {
        dropInRequest.DisablePayPal();
      }

      if (request.AndroidPayDisabled)
      {
        dropInRequest.DisableAndroidPay();
      }

      if (request.GooglePaymentDisabled)
      {
        dropInRequest.DisableGooglePayment();
      }

      if (false == request.PaypalDisabled)
      {
        dropInRequest.PaypalAdditionalScopes(new[] { PayPal.ScopeAddress });
      }

      StartActivityForResult(dropInRequest.GetIntent(this), DROP_IN_REQUEST);
    }

    static TaskCompletionSource<Services.DropInResult> showDropInTcs;
    public static void ShowDropIn(TaskCompletionSource<Services.DropInResult> tcs, Services.DropInRequest request)
    {
      showDropInTcs?.TrySetCanceled();
      showDropInTcs = tcs;

      var currentTopActivity = Plugin.CurrentActivity.CrossCurrentActivity
                                     .Current.Activity;
      var intent = new Intent(currentTopActivity, typeof(BraintreeDropInHandlerActivity));
      intent.PutExtra(DROP_IN_REQUEST_KEY, JsonConvert.SerializeObject(request));
      currentTopActivity.StartActivity(intent);
    }

    public void OnCancel(int p0)
    {
      Finish();

      showDropInTcs?.TrySetCanceled();
      showDropInTcs = null;
    }

    public void OnError(Java.Lang.Exception p0)
    {
      System.Diagnostics.Debug.WriteLine(p0.StackTrace);

      Finish();
      showDropInTcs?.TrySetException(p0);
      showDropInTcs = null;
    }

    public void OnPaymentMethodNonceCreated(PaymentMethodNonce p0)
    {
      System.Diagnostics.Debug.WriteLine(p0.Description);
    }

    public void OnResult(DropInResult p0)
    {
      var result = new Services.DropInResult
      {
        Description = p0.DeviceData,
        OptionType = p0.PaymentMethodType == PaymentMethodType.AndroidPay ? Services.PaymentOptionType.AndroidPay : Services.PaymentOptionType.Unknown,

        Method = new Services.PaymentMethod
        {
          Nonce = p0.PaymentMethodNonce.Nonce,
          LocalizedDescription = p0.PaymentMethodNonce.Description,
          IsDefault = p0.PaymentMethodNonce.IsDefault,
          Type = p0.PaymentMethodNonce.TypeLabel
        }
      };

      Finish();
      showDropInTcs?.TrySetResult(result);
      showDropInTcs = null;
    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Intent data)
    {
      base.OnActivityResult(requestCode, resultCode, data);

      if (resultCode == Result.Ok)
      {
        var result = (DropInResult)data.GetParcelableExtra(DropInResult.ExtraDropInResult);

        OnResult(result);
      }
      else if (resultCode == Result.Canceled)
      {
        OnCancel(0);
      }
      else
      {
        Finish();
        showDropInTcs?.TrySetException(new InvalidOperationException("Invalid ResultCode"));
        showDropInTcs = null;
      }
    }

    GooglePaymentRequest GetGooglePaymentRequest(Services.DropInRequest request)
    {
      return new GooglePaymentRequest()
          .InvokeTransactionInfo(
              TransactionInfo.NewBuilder()
                  .SetTotalPrice(request.Amount)
                  .SetCurrencyCode(request.CurrencyCode)
                  .SetTotalPriceStatus(WalletConstants.TotalPriceStatusFinal)
                  .Build())
              .EmailRequired(true);
    }

    Cart GetAndroidPayCart(Services.DropInRequest request)
    {
      //TODO Add items to request
      return Cart
          .NewBuilder()
              .SetCurrencyCode(request.CurrencyCode)
              .SetTotalPrice(request.Amount)
              .AddLineItem(LineItem.NewBuilder()
                      .SetCurrencyCode("USD")
                      .SetDescription("Description")
                      .SetQuantity("1")
                      .SetUnitPrice("1.00")
                      .SetTotalPrice("1.00")
                      .Build())
          .Build();
    }

  }
}