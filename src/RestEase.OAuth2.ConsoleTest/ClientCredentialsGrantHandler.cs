using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace RestEase.OAuth2.ConsoleTest
{
    class ClientCredentialsGrantHandler
    {
        private readonly SemaphoreSlim _accessTokenSemaphore = new SemaphoreSlim(1, 1);
        private readonly IOAuth2Api _api;

        public OAuth2Model OAuth2Model { get; set; }

        public ClientCredentialsGrantHandler(string url)
        {
            _api = RestClient.For<IOAuth2Api>(url);
        }

        public async Task<OAuth2Model> GetAccessTokenAsync()
        {
            await _accessTokenSemaphore.WaitAsync();

            try
            {
                if (OAuth2Model == null)
                {
                    var data = new Dictionary<string, object>
                    {
                        {"grant_type", "password"},
                        {"username", "u"},
                        {"password", "p"},
                        {"client_id", "X"},
                        {"client_secret", "P"},
                    };

                    OAuth2Model = await _api.AuthorizeAsync(data);
                }
                else
                {
                    var data = new Dictionary<string, object>
                    {
                        {"grant_type", "refresh_token"},
                        {"refresh_token", OAuth2Model.RefreshToken},
                        {"client_id", "X"},
                        {"client_secret", "P"},
                    };

                    OAuth2Model = await _api.RefreshTokenAsync(data);
                }

                return OAuth2Model;
            }
            finally
            {
                _accessTokenSemaphore.Release(1);
            }
        }

        public async Task<OAuth2Model> GetRefreshTokenAsync()
        {
            await _accessTokenSemaphore.WaitAsync();

            try
            {
                if (OAuth2Model == null)
                {
                    return await GetAccessTokenAsync();
                }
                else
                {
                    var data = new Dictionary<string, object>
                    {
                        {"grant_type", "refresh_token"},
                        {"refresh_token", OAuth2Model.RefreshToken},
                        {"client_id", "X"},
                        {"client_secret", "P"},
                    };

                    OAuth2Model = await _api.RefreshTokenAsync(data);
                }

                return OAuth2Model;
            }
            finally
            {
                _accessTokenSemaphore.Release(1);
            }
        }
    }
}
