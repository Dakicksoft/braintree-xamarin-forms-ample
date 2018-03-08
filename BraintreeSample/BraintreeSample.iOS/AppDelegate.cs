﻿using Foundation;
using IconEntry.FormsPlugin.iOS;
using Plugin.Toasts;
using Prism;
using Prism.Ioc;
using UIKit;
using UserNotifications;

namespace BraintreeSample.iOS
{
  // The UIApplicationDelegate for the application. This class is responsible for launching the 
  // User Interface of the application, as well as listening (and optionally responding) to 
  // application events from iOS.
  [Register("AppDelegate")]
  public partial class AppDelegate : global::Xamarin.Forms.Platform.iOS.FormsApplicationDelegate
  {
    //
    // This method is invoked when the application has loaded and is ready to run. In this 
    // method you should instantiate the window, load the UI into it and then make the window
    // visible.
    //
    // You have 17 seconds to return from this method, or iOS will terminate your application.
    //
    public override bool FinishedLaunching(UIApplication app, NSDictionary options)
    {
      global::Xamarin.Forms.Forms.Init();

      BraintreePayPal.BraintreePayPalLinker.Init();
      IconEntryRenderer.Init();
      ToastNotification.Init();

      LoadApplication(new App(new iOSInitializer()));


      // Request Permissions
      if (UIDevice.CurrentDevice.CheckSystemVersion(10, 0))
      {
        // Request Permissions
        UNUserNotificationCenter.Current.RequestAuthorization(UNAuthorizationOptions.Alert | UNAuthorizationOptions.Badge | UNAuthorizationOptions.Sound, (granted, error) =>
        {
          // Do something if needed
        });
      }
      else if (UIDevice.CurrentDevice.CheckSystemVersion(8, 0))
      {
        var notificationSettings = UIUserNotificationSettings.GetSettingsForTypes(
        UIUserNotificationType.Alert | UIUserNotificationType.Badge | UIUserNotificationType.Sound, null);

        app.RegisterUserNotificationSettings(notificationSettings);
      }

      return base.FinishedLaunching(app, options);
    }
  }

  public class iOSInitializer : IPlatformInitializer
  {
    public void RegisterTypes(IContainerRegistry containerRegistry)
    {
      containerRegistry.Register<Services.IDropInService, DropInService>();
      containerRegistry.Register<IToastNotificator, ToastNotification>();
    }
  }
}