using System;
using System.Diagnostics;
using System.Threading.Tasks;
using BraintreeSample.Providers.Interfaces;
using Plugin.Geolocator;
using Plugin.Geolocator.Abstractions;

namespace BraintreeSample.Providers
{
	public class LocationProvider : ILocationProvider
	{
		private readonly TimeSpan PositionReadTimeout = TimeSpan.FromSeconds(5);

		public async Task<ILocationResponse> GetPositionAsync()
		{
			try
			{
				var locator = CrossGeolocator.Current;
				locator.DesiredAccuracy = 50;

                var position = await CrossGeolocator.Current.GetPositionAsync(PositionReadTimeout);

				var geolocation = new GeoLocation
				{
					Latitude = position.Latitude,
					Longitude = position.Longitude,
				};

				return geolocation;
			}
			catch (GeolocationException geoEx)
			{
				Debug.WriteLine(geoEx);
			}
			catch (TaskCanceledException ex)
			{
				Debug.WriteLine(ex);
			}

			return new UnknownLocation();
		}
	}
}
