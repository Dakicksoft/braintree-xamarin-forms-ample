using Newtonsoft.Json;

namespace BraintreeSample.Services.Base
{
  public class AuthenticationResponse
  {


    public AuthenticationResponse()
    {

    }

    [JsonProperty("UserId")]
    public int UserId { get; set; }

    [JsonProperty("access_token")]
    public string AccessToken { get; set; }

    [JsonProperty("token_type")]
    public string TokenType { get; set; }

    [JsonProperty("expires_in")]
    public int ExpiresIn { get; set; }

    [JsonProperty("error")]
    public string Error { get; set; }

    [JsonProperty("error_description")]
    public string ErrorDescription { get; set; }

    [JsonProperty("FullName")]
    public string FullName { get; set; }

    [JsonProperty("Email")]
    public string Email { get; set; }

    [JsonProperty("Username")]
    public string Username { get; set; }

  }
}
