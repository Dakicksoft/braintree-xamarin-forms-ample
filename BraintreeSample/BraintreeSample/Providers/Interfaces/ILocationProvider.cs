using System;
using System.Threading.Tasks;

namespace BraintreeSample.Providers.Interfaces
{
	public interface ILocationProvider
	{
		Task<ILocationResponse> GetPositionAsync();
	}
}
