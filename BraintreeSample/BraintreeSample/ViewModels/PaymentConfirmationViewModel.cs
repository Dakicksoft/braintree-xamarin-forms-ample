using BraintreeSample.Services.Checkout;
using BraintreeSample.Services.Dialog;
using Prism.Navigation;

namespace BraintreeSample.ViewModels
{
	public class PaymentConfirmationViewModel : ViewModelBase
  {
    private readonly ICheckoutService _checkoutService;
    private readonly IDialogService _dialogService;

    public PaymentConfirmationViewModel(IDialogService dialogService, ICheckoutService checkoutService, INavigationService navigationService)
     : base(navigationService)
    {
      Title = "Payment Confirmation";
      _checkoutService = checkoutService;
      _dialogService = dialogService;
    }
  }
}
