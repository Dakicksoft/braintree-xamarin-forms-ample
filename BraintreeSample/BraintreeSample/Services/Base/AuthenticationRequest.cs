using Newtonsoft.Json;

namespace BraintreeSample.Services.Base
{
  public class AuthenticationRequest
  {
    [JsonProperty("username")]
    public string UserName { get; set; }

    [JsonProperty("password")]
    public string Password { get; set; }

    [JsonProperty("grant_type")]
    public string GrantType { get; set; }
  }
}
