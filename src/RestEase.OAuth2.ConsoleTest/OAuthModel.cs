using Newtonsoft.Json;

namespace RestEase.OAuth2.ConsoleTest
{
    public class OAuthModel
    {
        [JsonProperty("access_token")]
        public string AccessToken { get; set; }
    }
}