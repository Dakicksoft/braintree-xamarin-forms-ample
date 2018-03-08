using BraintreeSample.Services;
using BraintreeSample.Services.Checkout;
using BraintreeSample.Services.Dialog;
using Prism.Commands;
using Prism.Navigation;
using System.Threading.Tasks;

namespace BraintreeSample.ViewModels
{
  public class MainPageViewModel : ViewModelBase
  {

    private readonly ICheckoutService _checkoutService;
    private readonly IDropInService _dropInService;
    private readonly IDialogService _dialogService;
    string clientToken;
    string customerId = "";

    public MainPageViewModel(IDropInService dropInService, IDialogService dialogService, ICheckoutService checkoutService, INavigationService navigationService)
        : base(navigationService)
    {
      Title = "Payment";
      _checkoutService = checkoutService;
      _dialogService = dialogService;
      _dropInService = dropInService;

      RewardAmount = "40";
      PayAmount = "60";
    }


    public DelegateCommand PurchaseCommand => new DelegateCommand(async () => await Purchase());
    public DelegateCommand AddPaymentMethodCommand => new DelegateCommand(async () => await AddPaymentMethod());



    private string _rewardAmount;
    public string RewardAmount
    {
      get => _rewardAmount;
      set
      {
        SetProperty(ref _rewardAmount, value);
      }
    }

    private string _payAmount;
    public string PayAmount
    {
      get => _payAmount;
      set
      {
        SetProperty(ref _payAmount, value);
      }
    }

    private async Task AddPaymentMethod()
    {

      var amount = decimal.Parse(PayAmount) - decimal.Parse(RewardAmount);

      if (amount <= 0)
        return;

      var dropInRequest = new DropInRequest
      {
        Amount = amount.ToString(),
        ApplePayDisabled = true,
        PaypalDisabled = true,
        AndroidPayDisabled = true,
        GooglePaymentDisabled = true,
      };

      try
      {

        if (this.IsBusy)
          return;

        this.IsBusy = true;

        if (string.IsNullOrWhiteSpace(clientToken) || false == _dropInService.IsValidToken(clientToken))
        {
          var response = await _checkoutService.GetClientTokenAsync(customerId);

          if (response == null)
          {
            await _dialogService.ShowAlertAsync("Token Error", "Error", "Try Again");
          }

          clientToken = response["client_token"];
          customerId = response["customer_id"];


          dropInRequest.Authorization = clientToken;
        }

        var dropInResult = await _dropInService.ShowDropInAsync(dropInRequest).ContinueWith(x =>
        {
          return x.IsCompleted && x.IsFaulted == false && x.IsCanceled == false
                  ? x.Result : null;
        });

        if (dropInResult == null)
        {
          await _dialogService.ShowAlertAsync("Unexpected error!", "Error", "Try Again");
          return;
        }

        Helpers.Settings.Nonce = dropInResult.Method.Nonce;

      }
      catch (System.Exception ex)
      {
        await _dialogService.ShowAlertAsync("Unexpected error!", "Error", "Try Again");
      }
      finally
      {
        this.IsBusy = false;
      }
    }

    private async Task Purchase()
    {
      var amount = decimal.Parse(PayAmount) - decimal.Parse(RewardAmount);

      if (amount <= 0)
        return;

      if (string.IsNullOrEmpty(clientToken) && string.IsNullOrEmpty(customerId))
      {
        await _dialogService.ShowAlertAsync("Please first select payment method", "Error", "Try Again");
        return;
      }

      if (string.IsNullOrEmpty(Helpers.Settings.Nonce))
      {
        await _dialogService.ShowAlertAsync("Please first select payment method", "Error", "Try Again");
        return;
      }

      var dropInRequest = new DropInRequest
      {
        Amount = amount.ToString(),
        ApplePayDisabled = true,
        PaypalDisabled = true,
        AndroidPayDisabled = true,
        GooglePaymentDisabled = true,
        CurrencyCode = "USD"
      };

      try
      {

        if (this.IsBusy)
          return;

        this.IsBusy = true;

        var result = await _checkoutService.MakeTransctionAsync(amount, Helpers.Settings.Nonce, customerId, GlobalSettings.MERCHANT_ACCOUNT_ID);

        if (result)
        {
          await this.NavigationService.NavigateAsync($"{Screens.PaymentConfirmation}");
          //await _dialogService.ShowAlertAsync("Payment success!", "Info", "Ok");
        }
        else
        {
          await _dialogService.ShowAlertAsync("Payment error!", "Error", "Try Again");
        }
      }
      catch (System.Exception ex)
      {
        await _dialogService.ShowAlertAsync("Unexpected error!", "Error", "Try Again");
      }
      finally
      {
        this.IsBusy = false;
        clientToken = "";
      }
    }
  }
}
