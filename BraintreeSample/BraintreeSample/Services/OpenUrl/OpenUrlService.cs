using System;
using Xamarin.Forms;

namespace BraintreeSample.Services.OpenUrl
{
	public class OpenUrlService : IOpenUrlService
	{
		public void OpenUrl(string url)
		{
			Device.OpenUri(new Uri(url));
		}
	}
}
