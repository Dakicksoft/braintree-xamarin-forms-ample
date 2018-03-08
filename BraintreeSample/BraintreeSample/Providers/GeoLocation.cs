using System;
using BraintreeSample.Providers.Interfaces;

namespace BraintreeSample.Providers
{
	public class GeoLocation : ILocationResponse
	{
		public double Latitude { get; set; }

		public double Longitude { get; set; }
	}
}
