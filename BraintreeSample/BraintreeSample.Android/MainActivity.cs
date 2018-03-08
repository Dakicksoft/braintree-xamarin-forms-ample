using Acr.UserDialogs;
using Android.App;
using Android.Content.PM;
using Android.OS;
using IconEntry.FormsPlugin.Android;
using Plugin.Permissions;
using Plugin.Toasts;
using Prism;
using Prism.Ioc;
using System;

namespace BraintreeSample.Droid
{
  [Activity(Label = "BraintreeSample", Icon = "@drawable/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
  public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
  {
    protected override void OnCreate(Bundle bundle)
    {
      TabLayoutResource = Resource.Layout.Tabbar;
      ToolbarResource = Resource.Layout.Toolbar;

      base.OnCreate(bundle);

      global::Xamarin.Forms.Forms.Init(this, bundle);

      IconEntryRenderer.Init();
      UserDialogs.Init(this);
      ToastNotification.Init(this);

      LoadApplication(new App(new AndroidInitializer()));


      AppDomain.CurrentDomain.UnhandledException += CurrentDomainOnUnhandledException;

    }

    private static void CurrentDomainOnUnhandledException(object sender, UnhandledExceptionEventArgs args)
    {
      Exception e = (Exception)args.ExceptionObject;

      // log won't be available, because dalvik is destroying the process
      //Log.Debug (logTag, "MyHandler caught : " + e.Message);
      // instead, your err handling code shoudl be run:
      Console.WriteLine("========= MyHandler caught : " + e.Message);

    }

    protected override void OnActivityResult(int requestCode, Result resultCode, Android.Content.Intent data)
    {
      base.OnActivityResult(requestCode, resultCode, data);

    }

    public override void OnRequestPermissionsResult(int requestCode, string[] permissions, Android.Content.PM.Permission[] grantResults)
    {
      base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
      PermissionsImplementation.Current.OnRequestPermissionsResult(requestCode, permissions, grantResults);
    }
  }

  public class AndroidInitializer : IPlatformInitializer
  {
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
      // Register any platform specific implementations
      containerRegistry.Register<Services.IDropInService, DropInService>();
      containerRegistry.Register<IToastNotificator, ToastNotification>();
    }
  }
}

