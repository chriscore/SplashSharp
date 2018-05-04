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
    public class SplashClient : ISplashClient
    {
        public SplashClient(string splashBaseUrl)
            : this(splashBaseUrl, new HttpClient())
        { }

        public SplashClient(string splashBaseUrl, HttpClient client)
        {
            Client = client;
            SplashBaseUrl = new Uri(splashBaseUrl, UriKind.Absolute);
            CachedArgs = new Dictionary<string, string>();
        }
        
        protected internal static JsonSerializerSettings SplashJsonSerializerSettings => LazySerializerSettings.Value;
        private static readonly Lazy<JsonSerializerSettings> LazySerializerSettings = new Lazy<JsonSerializerSettings>(GetSerializerSettings);

        public Uri SplashBaseUrl { get; }
        public Dictionary<string, string> CachedArgs { get; set; }

        internal HttpClient Client { get; }

        // TODO: intercept x-splash-saved-arguments to store into CachedArgs, provide an interface to safely use this.
        private const string SavedArgumentsHeaderName = "X-Splash-Saved-Arguments";

        private const string RenderHtmlEndpoint = "render.html";
        private const string RenderHarEndpoint = "render.har";
        private const string RenderPngEndpoint = "render.png";
        private const string RenderJpegEndpoint = "render.jpeg";
        private const string RenderJsonEndpoint = "render.json";
        private const string ExecuteEndpoint = "execute";
        private const string RunEndpoint = "run";
        private const string InvokeGarbageCollectionEndpoint = "_gc";
        private const string GetInstanceStatusEndpoint = "_debug";
        private const string PingEndpoint = "_ping";
        
        /// <summary>
        /// Gets rendered HTML as HttpResponseMessage to do as you please with
        /// </summary>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> RenderHtmlAsync(RenderHtmlOptions options, CancellationToken token = default(CancellationToken))
        {
            return SendPostRequestWithVerification(RenderHtmlEndpoint, options, token);
        }

        /// <summary>
        /// Gets rendererd HTML presented as an HtmlAgilityPack HtmlDocument
        /// </summary>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Gets interaction metadata in HAR format
        /// </summary>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<SplashResponseWrapper<RenderHarResponse>> RenderHarAsync(RenderHarOptions options, CancellationToken token = default(CancellationToken))
        {
            return await MakeTypedPostRequestAsync<RenderHarResponse>(RenderHarEndpoint, options, token);
        }

        /// <summary>
        /// Gets the page rendered as a png image
        /// </summary>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> RenderPngAsync(RenderPngOptions options, CancellationToken token = default(CancellationToken))
        {
            var request = BuildSplashRequest(RenderPngEndpoint, options);
            return Client.SendAsync(request, token);
        }

        /// <summary>
        /// Gets the page rendered as a jpeg image
        /// </summary>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> RenderJpegAsync(RenderJpegOptions options, CancellationToken token = default(CancellationToken))
        {
            var request = BuildSplashRequest(RenderJpegEndpoint, options);
            return Client.SendAsync(request, token);
        }

        /// <summary>
        /// Returns a json-encoded dictionary with information about javascript-rendered webpage. 
        /// It can include HTML, PNG and other information, based on arguments passed.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public async Task<SplashResponseWrapper<RenderJsonResponse>> RenderJson(RenderJsonOptions options, CancellationToken token)
        {
            return await MakeTypedPostRequestAsync<RenderJsonResponse>(RenderJsonEndpoint, options, token);
        }

        /// <summary>
        /// Executes a custom rendering script and return a result.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        public Task<HttpResponseMessage> Execute(ExecuteOptions options, CancellationToken token)
        {
            return SendPostRequestWithVerification(ExecuteEndpoint, options, token);
        }

        public Task<HttpResponseMessage> Run(RunOptions options, CancellationToken token)
        {
            return SendPostRequestWithVerification(RunEndpoint, options, token);
        }

        /// <summary>
        /// Reclaims some RAM by calling the Python GC and clearing internal WebKit caches on the Splash server.
        /// This will likely clear cached arguments in CachedArgs
        /// </summary>
        /// <returns></returns>
        public async Task<SplashResponseWrapper<GarbageCollectionResponse>> InvokeGarbageCollection(CancellationToken token = default(CancellationToken))
        {
            return await MakeTypedPostRequestAsync<GarbageCollectionResponse>(InvokeGarbageCollectionEndpoint, null, token);
        }

        public async Task<SplashResponseWrapper<StatusResponse>> GetInstanceStatus(CancellationToken token = default(CancellationToken))
        {
            return await MakeTypedGetRequestAsync<StatusResponse>(GetInstanceStatusEndpoint, token);
        }

        public async Task<SplashResponseWrapper<PingResponse>> Ping(CancellationToken token = default(CancellationToken))
        {
            return await MakeTypedGetRequestAsync<PingResponse>(PingEndpoint, token);
        }

        private async Task<SplashResponseWrapper<T>> MakeTypedPostRequestAsync<T>(string endpoint, BaseRenderRequestOptions options, CancellationToken token)
        {
            var response = await SendPostRequestWithVerification(endpoint, options, token);
            var deserialized = await DeserializeResponse<T>(response);

            return deserialized;
        }
        
        private async Task<SplashResponseWrapper<T>> MakeTypedGetRequestAsync<T>(string endpoint, CancellationToken token)
        {
            var request = BuildSplashRequest(endpoint);
            var response = await Client.SendAsync(request, token);
            await VerifyHttpResponseMessageValid(response);

            var deserialized = await DeserializeResponse<T>(response);

            return deserialized;
        }

        private async Task<HttpResponseMessage> SendPostRequestWithVerification(string endpoint, BaseRenderRequestOptions options, CancellationToken token)
        {
            var request = BuildSplashRequest(endpoint, options);
            var response = await Client.SendAsync(request, token);
            await VerifyHttpResponseMessageValid(response);
            return response;
        }

        protected internal virtual async Task VerifyHttpResponseMessageValid(HttpResponseMessage response)
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
                    else
                    {
                        response.EnsureSuccessStatusCode();
                    }
                }
            }
        }

        protected internal virtual void SetCachedArgsFromResponse(HttpResponseMessage response)
        {
            throw new NotImplementedException();
            if (response.Headers.TryGetValues(SavedArgumentsHeaderName, out var headerValues))
            {

            }
        }

        protected virtual async Task<SplashResponseWrapper<T>> DeserializeResponse<T>(HttpResponseMessage response)
        {
            var wrapper = new SplashResponseWrapper<T>(response);

            var content = await response.Content.ReadAsStringAsync();
            var deserialized = JsonConvert.DeserializeObject<T>(content, SplashJsonSerializerSettings);
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
                var json = JsonConvert.SerializeObject(options, SplashJsonSerializerSettings);
                request.Content = new StringContent(json, Encoding.UTF8, "application/json");
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

            settings.Converters.Add(new NullableBoolJsonConverter());
            settings.Converters.Add(new LuaScriptOptionsJsonConverter<ExecuteOptions>());
            settings.Converters.Add(new LuaScriptOptionsJsonConverter<RunOptions>());
            return settings;
        }
    }
}
