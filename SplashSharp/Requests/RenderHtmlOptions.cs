using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SplashSharp.Requests
{
    public class RenderHtmlOptions : BaseSplashRequestOptions
    {
        // The URL to render (required)
        [JsonRequired]
        public string Url { get; set; }

        // The base URL to use for relative referenced resources
        public string BaseUrl { get; set; }

        // The max request timeout in seconds. Defaults to 30, default maximum allowed by splash is 90.
        // Can be overridden by starting splash with --max-timeout switch
        public double? Timeout { get; set; }

        // Timeout for requests for resources
        public double? ResourceTimeout { get; set; }

        // Time in seconds to wait for updates after a page is loaded. Defaults to 0
        public double? Wait { get; set; }

        // Proxy profile name or proxy URL. URLs must have format [protocol://][user:password@]proxyhost[:port]
        // Proxy URLs must have protocol http or socks5. Port defaults to 1080
        public string Proxy { get; set; }

        // Javascript profile name
        public string Js { get; set; }

        // Javascript code to be executed in page context
        public string JsSource { get; set; }

        // Resources to be request-filtered. Comma separated list
        public string Filters { get; set; }

        // List of allowed domains. Comma separated list.
        public string AllowedDomains { get; set; }

        // Comma separated list of allowed resource content types. Uses fnmatch
        public string AllowedContentTypes { get; set; }

        // Comma separated list of forbidden resource content types. Uses fnmatch
        public string ForbiddenContentTypes { get; set; }

        // Whether to render images. Cached images may still be rendered
        public bool? Images { get; set; }

        // Headers to send with the initial outgoing request
        // The “User-Agent” header is special: is is used for all outgoing requests, unlike other headers
        // Default content type for POST requests is application/x-www-form-urlencoded
        public Dictionary<string, string> Headers { get; set; }

        // The body of the HTTP POST request to be sent if method is POST. 
        // Default content type for POST is application/x-www-form-urlencoded
        public string Body { get; set; }

        // The HTTP method for the outbound splash request. Options: GET, POST. Default is GET
        public string HttpMethod { get; set; }

        // Stores values in a splash cache. Useful for reducing overhead, such as values for the Js param.
        // Used with Load_Args
        // TODO: Exposed in client SavedArgs property, splash references this using an SHA1 hash (the value in the dictionary)
        public Dictionary<string, string> SaveArgs { get; set; }

        // A dictionary of <argument_name, SHA1_hash>
        // TODO: If any arguments are not found, splash responds with HTTP 498. In this case client should repeat the request, but use save_args and send full argument values
        // Splash uses LRU cache to store values; the number of entries is limited, and cache is cleared after each Splash restart
        public Dictionary<string, string> LoadArgs { get; set; }

        // Whether to enable HTML5 media. Splash default is false, and may cause instability if enabled
        [JsonProperty("html5_media")]
        public bool? Html5Media { get; set; }
    }
}
