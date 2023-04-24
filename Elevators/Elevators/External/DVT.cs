using Elevators.Models.Request;
using Elevators.Models.Response;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Elevators.External
{
    internal class DVT
    {
        private readonly Dictionary<string, string> _config;
        private static AuthCache? _auth;

        public DVT(Dictionary<string, string> config)
        {
            _config = config;
        }

        public async Task ElevatorFailure(int elevator, string message)
        {
            try
            {
                var client = await CreateClient();
                
                var response = await client.PostAsJsonAsync($"{_config["DVT_API"]}/notifications/failure", new FailureRequest {
                    Elevator = elevator,
                    Message = message
                });

                if (!response.IsSuccessStatusCode)
                    throw new Exception($"DVT notification return an error response. Error Code: {response.StatusCode}");
            }
            catch(Exception ex) { 
                Console.WriteLine($"DVT_ElevatorFailure: Failed to send notification. Error: {ex.GetBaseException().Message}"); 
            }
        }

        private async Task ElevatorMaintenance(int elevator, string message)
        {
            await Task.Delay(0);
            throw new NotImplementedException();
        }

        private async Task<HttpClient> CreateClient()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Add("SubscriptionKey", _config["SubscriptionKey"]);

            if (_auth == null || _auth.ExpiryDate <= DateTime.Now)
                _auth = await Login(client);

            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {_auth?.Token}");

            return client;
        }

        private async Task<AuthCache> Login(HttpClient client)
        {
            var response = await client.PostAsJsonAsync(_config["DVT_API"], new {
                username = _config["DvtUsername"],
                password = _config["DvtPassword"]
            });

            var content = await response.Content.ReadAsStringAsync();
            var authToken = JsonConvert.DeserializeObject<AuthToken>(content);

            return new AuthCache(authToken);
        }
    }

    public class AuthCache
    {
        public string Token { get; set; }
        public DateTime ExpiryDate { get; set; }

        public AuthCache(AuthToken? token) {
            ExpiryDate = DateTime.Now.AddSeconds(token?.ExpiresIn ?? 0);
            Token = token.Token;
        }
    }
}
