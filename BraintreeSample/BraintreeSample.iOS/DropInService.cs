using System.Threading.Tasks;
using BraintreeDropIn;
using BraintreeUIKit;
using Foundation;
using UIKit;
using Acr.Support.iOS;
using BraintreeSample.Services;

namespace BraintreeSample.iOS
{
  public class DropInService : Services.IDropInService
  {
    public Task<Services.DropInResult> ShowDropInAsync(Services.DropInRequest request)
    {
      var dropInRequest = new BTDropInRequest
      {
        Amount = request.Amount,
        ApplePayDisabled = request.ApplePayDisabled,
        PaypalDisabled = request.PaypalDisabled,
        VenmoDisabled = request.VenmoDisabled,
        ShouldMaskSecurityCode = request.ShouldMaskSecurityCode,
        ThreeDSecureVerification = request.ThreeDSecureVerification == true
      };

      switch (request.Theme)
      {
        case DropInTheme.Dark:
          BTUIKAppearance.DarkTheme();
          break;
        case DropInTheme.Light:
          BTUIKAppearance.LightTheme();
          break;
      }

      var tcs = new TaskCompletionSource<DropInResult>();
      var dropinVc = new BTDropInController(request.Authorization, dropInRequest, (dropInController, dropInResult, error) =>
      {
        dropInController.DismissViewController(true, null);

        if (error != null)
        {
          tcs.TrySetException(new NSErrorException(error));
          return;
        }

        if (dropInResult.Cancelled)
        {
          tcs.TrySetCanceled();
          return;
        }

        var result = new DropInResult
        {
          OptionType = (PaymentOptionType)dropInResult.PaymentOptionType,
          Description = dropInResult.PaymentDescription,
          Method = new PaymentMethod
          {
            Nonce = dropInResult.PaymentMethod.Nonce,
            LocalizedDescription = dropInResult.PaymentMethod.LocalizedDescription,
            IsDefault = dropInResult.PaymentMethod.IsDefault,
            Type = dropInResult.PaymentMethod.Type
          }
        };

        tcs.TrySetResult(result);
      });

      var topVc = UIApplication.SharedApplication.GetTopViewController();
      topVc.PresentViewController(dropinVc, true, null);

      return tcs.Task;
    }

    public bool IsValidToken(string clientToken)
    {
      return true;
    }
  }
}
