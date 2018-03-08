using System;
using Plugin.Settings;
using Plugin.Settings.Abstractions;


namespace BraintreeSample.Helpers
{
	/// <summary>
	/// This is the Settings static class that can be used in your Core solution or in any
	/// of your client applications. All settings are laid out the same exact way with getters
	/// and setters. 
	/// </summary>
	public static class Settings
	{
		private static ISettings AppSettings => CrossSettings.Current;

	  #region Setting Constants

		private const string IdUseMocks = "use_mocks";
		private const string IdUrlBase = "url_base";
		private const string IdUseFakeLocation = "use_fake_location";
		private const string IdLatitude = "latitude";
		private const string IdLongitude = "longitude";
		private const string IdAllowGpsLocation = "allow_gps_location";
    private const string IdToggleOnline = "toggle_online";
	  private const string NonceKey = "nonce";

    private static readonly bool UseMocksDefault = true;
		private static readonly bool UseFakeLocationDefault = false;
		private static readonly bool AllowGpsLocationDefault = false;
		private static readonly double FakeLatitudeDefault = 47.604610d;
		private static readonly double FakeLongitudeDefault = -122.315752d;
    private static readonly string NonceDefault = string.Empty;
	  private static readonly bool  ToggleOnlineDefault = false;
	  private static readonly string AuthTokenDefault = string.Empty;
    #endregion


    public static bool UseMocks
		{
			get => AppSettings.GetValueOrDefault(IdUseMocks, UseMocksDefault);
			set => AppSettings.AddOrUpdateValue(IdUseMocks, value);
		}


		public static bool UseFakeLocation
		{
			get => AppSettings.GetValueOrDefault(IdUseFakeLocation, UseFakeLocationDefault);
			set => AppSettings.AddOrUpdateValue(IdUseFakeLocation, value);
		}

		public static string Latitude
		{
			get => AppSettings.GetValueOrDefault(IdLatitude, FakeLatitudeDefault.ToString());
			set => AppSettings.AddOrUpdateValue(IdLatitude, value);
		}

		public static string Longitude
		{
			get => AppSettings.GetValueOrDefault(IdLongitude, FakeLongitudeDefault.ToString());
			set => AppSettings.AddOrUpdateValue(IdLongitude, value);
		}

		public static bool AllowGpsLocation
		{
			get => AppSettings.GetValueOrDefault(IdAllowGpsLocation, AllowGpsLocationDefault);
			set => AppSettings.AddOrUpdateValue(IdAllowGpsLocation, value);
		}

	  public static bool ToggleOnline
    {
	    get => AppSettings.GetValueOrDefault(IdToggleOnline, ToggleOnlineDefault);
	    set => AppSettings.AddOrUpdateValue(IdToggleOnline, value);
	  }

	

	  public static string Nonce
	  {
	    get => AppSettings.GetValueOrDefault(NonceKey, NonceDefault);
	    set => AppSettings.AddOrUpdateValue(NonceKey, value);
	  }



  }
}
