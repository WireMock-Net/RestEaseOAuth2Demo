using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RestEase.OAuth2.ConsoleTest
{
    class ClientCredentialsGrant
    {
        private readonly SemaphoreSlim _accesTokenSemaphore = new SemaphoreSlim(1, 1);
        private readonly IOAuth2Api _api;

        public OAuth2Model OAuth2Model { get; set; }

        public ClientCredentialsGrant(string url)
        {
            _api = RestClient.For<IOAuth2Api>(url);
        }

        public async Task<OAuth2Model> GetAccessTokenAsync()
        {
            await _accesTokenSemaphore.WaitAsync();

            try
            {
                if (OAuth2Model == null)
                {
                    var data = new Dictionary<string, object>
                    {
                        {"grant_type", "password"},
                        {"username", "u"},
                        {"password", "p"}
                    };

                    OAuth2Model = await _api.AuthenticateAsync(data);
                }

                return OAuth2Model;
            }
            finally
            {
                _accesTokenSemaphore.Release(1);
            }
        }
    }
}
