using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading;
using System.Threading.Tasks;
using RestEaseOAuth2Test;

namespace RestEase.OAuth2.ConsoleTest
{
    public interface IOAuth2
    {
        [Post("/oauth2/access")]
        Task<OAuthModel> AuthenticateAsync([Body(BodySerializationMethod.UrlEncoded)] Dictionary<string, object> data);
    }

    class MyClass
    {
        private readonly SemaphoreSlim OAuthModelSemaphore = new SemaphoreSlim(1, 1);
        private readonly IOAuth2 _api;

        public MyClass()
        {
            _api = RestClient.For<IOAuth2>("http://localhost:9090");
        }

        public async Task<OAuthModel> GetAccessTokenAsync()
        {
            await OAuthModelSemaphore.WaitAsync();

            try
            {
                if (OAuthModel == null)
                {
                    var data = new Dictionary<string, object>
                    {
                        {"grant_type", "password"},
                        {"username", "u"},
                        {"password", "p"}
                    };

                    OAuthModel = await _api.AuthenticateAsync(data);
                }

                return OAuthModel;
            }
            finally
            {
                OAuthModelSemaphore.Release(1);
            }
        }

        public OAuthModel OAuthModel { get; set; }
    }

    class Program
    {
        static void Main(string[] args)
        {
            MyClass context = new MyClass();
            IMyApp api = RestClient.For<IMyApp>("http://localhost:9090", (request, token) => RequestModifier(context, request, token));

            string result = api.GetHelloAsync().Result;

            Console.WriteLine(result);

            int x = 0;
        }

        private static async Task RequestModifier(MyClass x, HttpRequestMessage request, CancellationToken token)
        {
            var auth = request.Headers.Authorization;
            if (auth != null)
            {
                var model = await x.GetAccessTokenAsync();
                //var value = Convert.ToBase64String(Encoding.ASCII.GetBytes("a:b"));
                request.Headers.Authorization = new AuthenticationHeaderValue(auth.Scheme, model.AccessToken);
            }
        }
    }
}