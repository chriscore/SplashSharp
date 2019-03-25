using System.Collections.Generic;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using HtmlAgilityPack;
using SplashSharp.Models;
using SplashSharp.Requests;

namespace SplashSharp
{
    public interface ISplashClient
    {
        /// <summary>
        /// A dictionary of arguments cached by SaveArgs options parameter
        /// </summary>
        Dictionary<string, string> CachedArgs { get; set; }

        /// <summary>
        /// Gets rendered HTML as HttpResponseMessage to do as you please with
        /// </summary>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> RenderHtmlAsync(RenderHtmlOptions options, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Gets rendererd HTML presented as an HtmlAgilityPack HtmlDocument
        /// </summary>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<SplashResponseWrapper<HtmlDocument>> RenderHtmlDocumentAsync(RenderHtmlOptions options, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Gets interaction metadata in HAR format
        /// </summary>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<SplashResponseWrapper<RenderHarResponse>> RenderHarAsync(RenderHarOptions options, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Gets the page rendered as a png image
        /// </summary>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> RenderPngRawAsync(RenderPngOptions options, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Gets the page rendered as a jpeg image
        /// </summary>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> RenderJpegRawAsync(RenderJpegOptions options, CancellationToken token = default(CancellationToken));

        /// <summary>
        /// Returns a json-encoded dictionary with information about javascript-rendered webpage. 
        /// It can include HTML, PNG and other information, based on arguments passed.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<SplashResponseWrapper<RenderJsonResponse>> RenderJson(RenderJsonOptions options, CancellationToken token);

        /// <summary>
        /// Executes a custom rendering script and return a result.
        /// </summary>
        /// <param name="options"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task<HttpResponseMessage> Execute(ExecuteOptions options, CancellationToken token);

        Task<HttpResponseMessage> Run(RunOptions options, CancellationToken token);

        /// <summary>
        /// Reclaims some RAM by calling the Python GC and clearing internal WebKit caches on the Splash server.
        /// This will likely clear cached arguments in CachedArgs
        /// </summary>
        /// <returns></returns>
        Task<SplashResponseWrapper<GarbageCollectionResponse>> InvokeGarbageCollection(CancellationToken token = default(CancellationToken));

        Task<SplashResponseWrapper<StatusResponse>> GetInstanceStatus(CancellationToken token = default(CancellationToken));
        Task<SplashResponseWrapper<PingResponse>> Ping(CancellationToken token = default(CancellationToken));
    }
}