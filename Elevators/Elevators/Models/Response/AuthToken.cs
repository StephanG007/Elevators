using Newtonsoft.Json;

namespace Elevators.Models.Response
{
    public class AuthToken
    {
        [JsonProperty("expires_in")]
        public int ExpiresIn { get; set; }
        [JsonProperty("token")]
        public string Token { get; set; }
    }
}
