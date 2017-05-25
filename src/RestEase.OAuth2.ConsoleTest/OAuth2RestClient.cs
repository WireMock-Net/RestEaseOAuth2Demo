using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;

namespace RestEase.OAuth2.ConsoleTest
{
    static class OAuth2RestClient
    {
        public static T For<T>(string url, ClientCredentialsGrantHandler grantHandlerType)
        {
            return RestClient.For<T>(url, (request, token) => RequestModifier(grantHandlerType, request, token));
        }

        private static async Task RequestModifier(ClientCredentialsGrantHandler x, HttpRequestMessage request, CancellationToken token)
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