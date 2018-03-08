using System.Collections.Generic;
using System.Threading.Tasks;
using Acr.UserDialogs;
using System.Threading;
using Plugin.Toasts;

namespace BraintreeSample.Services.Dialog
{
  public class DialogService : IDialogService
  {
    private readonly IToastNotificator _toastNotificator;

    public DialogService()
    {
      //_toastNotificator = toastNotificator;
    } 


    public void HideLoading()
    {
      UserDialogs.Instance.HideLoading();
    }

    public Task ShowAlertAsync(string message, string title, string buttonLabel)
    {
      return UserDialogs.Instance.AlertAsync(message, title, buttonLabel);
    }
    public void ShowError(string message, int timeout = 2000)
    {
      var config = new ActionSheetConfig();
      config.Cancel = new ActionSheetOption("Cancel");
      //config.Destructive = new ActionSheetOption("Try Again");
     // config.ItemIcon = "ic_alert_circle_grey600_24dp";
      config.Title = "Error";
      config.SetMessage(message);
      UserDialogs.Instance.ActionSheet(config);
    }
    public void ShowLoading(string title = null, MaskType? maskType = null)
    {
      UserDialogs.Instance.ShowLoading(title, maskType);

    }
    public void ShowSuccess(string message, int timeoutMillis = 2000)
    {
      var config = new ActionSheetConfig();
      config.Cancel = new ActionSheetOption("Cancel");
      //config.Destructive = new ActionSheetOption("Desc");
    //  config.ItemIcon = "ic_alert_circle_grey600_24dp";
      config.Message = message;
      //config.Title = "S";
      UserDialogs.Instance.ActionSheet(config);
    }
    public Task<bool> ConfirmAsync(ConfirmConfig config, CancellationToken? cancelToken = default(CancellationToken?))
    {
      return UserDialogs.Instance.ConfirmAsync(config, cancelToken);
    }
    public Task<bool> ConfirmAsync(string message, string title = null, string okText = null, string cancelText = null, CancellationToken? cancelToken = default(CancellationToken?))
    {
      return UserDialogs.Instance.ConfirmAsync(message, title, okText, cancelText, cancelToken);
    }
    public void ShowNotifaciton(ToastNotificationTypeEnum type, string message, int delay = 2000)
    {
      var config = new ToastConfig(message);

      config.SetPosition(ToastPosition.Top);
      config.SetDuration(delay);
      //config.SetBackgroundColor(System.Drawing.Color.Aqua);

      //switch (type)
      //{
      //  case ToastNotificationTypeEnum.Error:
      //    config.SetBackgroundColor(new System.Drawing.Color());
      //    config.SetMessageTextColor(System.Drawing.Color.White)
      //    break;
      //  case ToastNotificationTypeEnum.Succuess:
      //    config.SetBackgroundColor(System.Drawing.Color.Firebrick);
      //    config.SetMessageTextColor(System.Drawing.Color.White)
      //    break;
      //  case ToastNotificationTypeEnum.Info:
      //    config.SetBackgroundColor(System.Drawing.Color.Firebrick);
      //    config.SetMessageTextColor(System.Drawing.Color.White)
      //    break;
      //  case ToastNotificationTypeEnum.Warning:
      //    config.SetBackgroundColor(System.Drawing.Color.Firebrick);
      //    config.SetMessageTextColor(System.Drawing.Color.White)
      //    break;
      //}

      UserDialogs.Instance.Toast(config);
    }
    public void Progress(string title, bool show, MaskType? maskType = null)
    {
      UserDialogs.Instance.Progress(title, null, null, show, maskType);

    }

    public async Task ShowToastAsync(string title, string description, NotificationAction action)
    {
      var options = new NotificationOptions()
      {
        Title = title,
        Description = description,
        IsClickable =false,
      };

      var result = await _toastNotificator.Notify(options);
    }
  }
  public enum ToastNotificationTypeEnum
  {
    Succuess,
    Warning,
    Error,
    Info
  }
}
