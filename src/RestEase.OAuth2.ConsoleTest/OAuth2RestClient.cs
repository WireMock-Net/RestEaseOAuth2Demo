using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace RestEase.OAuth2.ConsoleTest
{
    static class OAuth2RestClient
    {
        public static T For<T>(string url, ClientCredentialsGrant grantType)
        {
            return RestClient.For<T>(url, (request, token) => RequestModifier(grantType, request, token));
        }

        private static async Task RequestModifier(ClientCredentialsGrant x, HttpRequestMessage request, CancellationToken token)
        {
            var auth = request.Headers.Authorization;
            if (auth != null)
            {
                var model = await x.GetAccessTokenAsync();
                request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, model.AccessToken);
            }
        }
    }
}