using Newtonsoft.Json;
using RestEase;
using WireMock.Client;
using WireMock.Matchers;
using WireMock.Net.StandAlone;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;

namespace WireMock.Net.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var server = StandAloneApp.Start(args);
            server.AllowPartialMapping();

            server
                .Given(Request.Create()
                    .WithPath("/oauth2/authorize")
                    .WithHeader("Content-Type", "application/x-www-form-urlencoded")
                    .UsingPost()
                    .WithBody("grant_type=password;username=u;password=p;client_id=X;client_secret=P")
                    )

                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(new { access_token = "AT", refresh_token = "RT" }));

            server
                .Given(Request.Create()
                    .WithPath("/oauth2/token")
                    .WithHeader("Content-Type", "application/x-www-form-urlencoded")
                    .UsingPost()
                    .WithBody("grant_type=refresh_token;refresh_token=RT;client_id=X;client_secret=P"))
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithHeader("Content-Type", "application/json")
                    .WithBodyAsJson(new { access_token = "AT2" }));

            server
                .Given(Request.Create()
                    .WithPath("/helloworld")
                    .UsingGet()
                    .WithHeader("Authorization", new RegexMatcher("^(?i)Bearer AT(2).$")))
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBody("hi"));

            // Create an implementation of the IFluentMockServerAdmin and pass in the base URL for the API.
            var api = RestClient.For<IFluentMockServerAdmin>("http://localhost:9090");
            var mappings = api.GetMappingsAsync().Result;
            System.Console.WriteLine($"mappings = {JsonConvert.SerializeObject(mappings)}");

            System.Console.WriteLine("Press any key to stop the server");
            System.Console.ReadKey();
        }
    }
}