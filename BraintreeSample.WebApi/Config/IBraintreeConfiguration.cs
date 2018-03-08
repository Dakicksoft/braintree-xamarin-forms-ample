using System;
using System.Collections.Generic;
using System.Linq;
using Braintree;

namespace BraintreeSample.WebApi.Config
{
  public interface IBraintreeConfiguration
  {
    IBraintreeGateway CreateGateway();
    string GetConfigurationSetting(string setting);
    IBraintreeGateway GetGateway();
  }
}
