using System;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SplashSharp.Requests;
using SplashSharp.Serialization;

namespace SplashSharp
{
    public class SplashClient
    {
        public SplashClient(string splashBaseUrl)
            : this(splashBaseUrl, new HttpClient())
        { }

        public SplashClient(string splashBaseUrl, HttpClient client)
        {
            Client = client;
            SplashBaseUrl = splashBaseUrl;

            JsonSettings = new JsonSerializerSettings()
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver()
                {
                    NamingStrategy = new SnakeCaseNamingStrategy(false, true)
                }
            };

            JsonSettings.Converters.Add(new NullableBoolConverter());
        }

        public string SplashBaseUrl { get; }

        private HttpClient Client { get; }
        private JsonSerializerSettings JsonSettings { get; }

        private const string RenderHtmlEndpoint = "render.html";

        public Task<HttpResponseMessage> GetHtmlAsync(RenderHtmlOptions options, CancellationToken token = default(CancellationToken))
        {
            var request = BuildSplashRequest(RenderHtmlEndpoint, options);
            return Client.SendAsync(request, token);
        }

        public async Task<HtmlDocument> GetHtmlDocumentAsync(RenderHtmlOptions options, CancellationToken token = default(CancellationToken))
        {
            var document = new HtmlDocument();

            // TODO: check for response error
            var response = await GetHtmlAsync(options, token);
            var responseStream = await response.Content.ReadAsStreamAsync();

            document.Load(responseStream);
            return document;
        }

        private HttpRequestMessage BuildSplashRequest(string endpoint, BaseSplashRequestOptions options)
        {
            var builder = new UriBuilder(SplashBaseUrl) {Path = endpoint};

            var requestUri = builder.ToString();
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri)
            {
                Content = new StringContent(JsonConvert.SerializeObject(options, JsonSettings), Encoding.UTF8, "application/json")
            };

            return request;
        }
    }
}
