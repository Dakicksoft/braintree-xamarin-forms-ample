using Prism;
using Prism.Ioc;
using BraintreeSample.Views;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using Prism.Autofac;

[assembly: XamlCompilation(XamlCompilationOptions.Compile)]
namespace BraintreeSample
{
  public partial class App : PrismApplication
  {
    /* 
     * The Xamarin Forms XAML Previewer in Visual Studio uses System.Activator.CreateInstance.
     * This imposes a limitation in which the App class must have a default constructor. 
     * App(IPlatformInitializer initializer = null) cannot be handled by the Activator.
     */
    public App() : this(null) { }

    public App(IPlatformInitializer initializer) : base(initializer) { }

    protected override async void OnInitialized()
    {
      InitializeComponent();

      await this.NavigationService.NavigateAsync($"{Screens.MasterDetailContainer}/NavigationPage/{Screens.MainPage}");
    }

    protected override void RegisterTypes(IContainerRegistry containerRegistry)
    {
      containerRegistry.RegisterForNavigation<NavigationPage>();
      containerRegistry.RegisterForNavigation<MainPage>();
      containerRegistry.RegisterForNavigation<MasterDetailContainer>();
      containerRegistry.RegisterForNavigation<PaymentConfirmation>();

      containerRegistry.RegisterInstance<Services.Dialog.IDialogService>(new Services.Dialog.DialogService());
      containerRegistry.RegisterInstance<Services.Media.IMediaService>(new Services.Media.MediaService());
      containerRegistry.RegisterInstance<Services.OpenUrl.IOpenUrlService>(new Services.OpenUrl.OpenUrlService());

      containerRegistry.RegisterInstance<Services.Checkout.ICheckoutService>(new Services.Checkout.CheckoutService(new Services.RequestProvider.RequestProvider()));
    }
  }
}
