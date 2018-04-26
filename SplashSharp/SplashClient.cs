using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SplashSharp.Models;
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
            CachedArgs = new Dictionary<string, string>();
        }
        
        protected internal static JsonSerializerSettings SplashJsonSerializerSettings => LazySerializerSettings.Value;
        private static readonly Lazy<JsonSerializerSettings> LazySerializerSettings = new Lazy<JsonSerializerSettings>(GetSerializerSettings);

        public string SplashBaseUrl { get; }
        public Dictionary<string, string> CachedArgs { get; set; }

        private HttpClient Client { get; }
        // TODO: intercept x-splash-saved-arguments to store into CachedArgs, provide an interface to safely use this.
        private const string SavedArgumentsHeaderName = "X-Splash-Saved-Arguments";

        private const string RenderHtmlEndpoint = "render.html";
        private const string RenderHarEndpoint = "render.har";
        private const string InvokeGarbageCollectionEndpoint = "_gc";
        private const string GetInstanceStatusEndpoint = "_debug";
        private const string PingEndpoint = "_ping";
        
        public Task<HttpResponseMessage> RenderHtmlAsync(RenderHtmlOptions options, CancellationToken token = default(CancellationToken))
        {
            var request = BuildSplashRequest(RenderHtmlEndpoint, options);
            return MakeRequestAsync(request, token);
        }

        public async Task<SplashResponseWrapper<HtmlDocument>> RenderHtmlDocumentAsync(RenderHtmlOptions options, CancellationToken token = default(CancellationToken))
        {
            var response = await RenderHtmlAsync(options, token);
            var responseStream = await response.Content.ReadAsStreamAsync();

            var document = new HtmlDocument();
            document.Load(responseStream);

            var wrapper = new SplashResponseWrapper<HtmlDocument>(response)
            {
                Data = document
            };

            return wrapper;
        }
        
        public async Task<SplashResponseWrapper<RenderHarResponse>> RenderHarAsync(RenderHarOptions options, CancellationToken token = default(CancellationToken))
        {
            var request = BuildSplashRequest(RenderHarEndpoint, options);
            var response = await MakeRequestAsync(request, token);

            var deserialized = await DeserializeResponse<RenderHarResponse>(response);

            return deserialized;
        }

        /// <summary>
        /// Reclaims some RAM by calling the Python GC and clearing internal WebKit caches on the Splash server.
        /// This will likely clear cached arguments in CachedArgs
        /// </summary>
        /// <returns></returns>
        public async Task<SplashResponseWrapper<GarbageCollectionResponse>> InvokeGarbageCollection(CancellationToken token = default(CancellationToken))
        {
            var request = BuildSplashRequest(InvokeGarbageCollectionEndpoint, null);
            var response = await MakeRequestAsync(request, token);

            var deserialized = await DeserializeResponse<GarbageCollectionResponse>(response);

            return deserialized;
        }

        public async Task<SplashResponseWrapper<StatusResponse>> GetInstanceStatus(CancellationToken token = default(CancellationToken))
        {
            var request = BuildSplashRequest(GetInstanceStatusEndpoint);
            var response = await MakeRequestAsync(request, token);

            var deserialized = await DeserializeResponse<StatusResponse>(response);

            return deserialized;
        }

        public async Task<SplashResponseWrapper<PingResponse>> Ping(CancellationToken token = default(CancellationToken))
        {
            var request = BuildSplashRequest(PingEndpoint);
            var response = await MakeRequestAsync(request, token);

            var deserialized = await DeserializeResponse<PingResponse>(response);

            return deserialized;
        }

        /// <summary>
        /// Handles making an HTTP request and optionally running VerifyHttpResponseMessageValid() on it.
        /// </summary>
        /// <param name="request">The request to execute</param>
        /// <param name="token">Cancellation token</param>
        /// <returns></returns>
        private async Task<HttpResponseMessage> MakeRequestAsync(HttpRequestMessage request, CancellationToken token)
        {
            var response = await Client.SendAsync(request, token);
            await VerifyHttpResponseMessageValid(response);

            return response;
        }

        protected virtual async Task VerifyHttpResponseMessageValid(HttpResponseMessage response)
        {
            if (!response.IsSuccessStatusCode)
            {
                using (var content = response.Content)
                {
                    if (content != null)
                    {
                        var responseStr = await content.ReadAsStringAsync();
                        throw new SplashWebException(response, responseStr);
                    }
                }
            }
        }

        protected virtual async Task<SplashResponseWrapper<T>> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var wrapper = new SplashResponseWrapper<T>(response);

            var content = await response.Content.ReadAsStringAsync();
            var deserialized = JsonConvert.DeserializeObject<T>(content);
            wrapper.Data = deserialized;

            return wrapper;
        }

        internal HttpRequestMessage BuildSplashRequest(string endpoint)
        {
            var builder = new UriBuilder(SplashBaseUrl) { Path = endpoint };

            var requestUri = builder.ToString();
            var request = new HttpRequestMessage(HttpMethod.Get, requestUri);
            
            return request;
        }

        internal HttpRequestMessage BuildSplashRequest(string endpoint, BaseRenderRequestOptions options)
        {
            var builder = new UriBuilder(SplashBaseUrl) {Path = endpoint};

            var requestUri = builder.ToString();
            var request = new HttpRequestMessage(HttpMethod.Post, requestUri);

            if (options != null)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(options, SplashJsonSerializerSettings), Encoding.UTF8, "application/json");
            }

            return request;
        }

        private static JsonSerializerSettings GetSerializerSettings()
        {
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore,
                ContractResolver = new DefaultContractResolver
                {
                    NamingStrategy = new SnakeCaseNamingStrategy()
                }
            };

            settings.Converters.Add(new NullableBoolConverter());
            return settings;
        }
    }
}
