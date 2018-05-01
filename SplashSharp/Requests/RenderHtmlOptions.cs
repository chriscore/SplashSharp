using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace SplashSharp.Requests
{
    public class RenderHtmlOptions : BaseRenderRequestOptions
    {
        // The URL to render (required)
        [JsonRequired]
        public string Url { get; set; }

        // The base URL to use for relative referenced resources
        public string BaseUrl { get; set; }

        // Timeout for requests for resources
        public double? ResourceTimeout { get; set; }

        // Time in seconds to wait for updates after a page is loaded. Defaults to 0
        public double? Wait { get; set; }

        // Javascript profile name
        public string Js { get; set; }

        // Javascript code to be executed in page context
        public string JsSource { get; set; }

        // Comma separated list of allowed resource content types. Uses fnmatch
        public string AllowedContentTypes { get; set; }

        // Comma separated list of forbidden resource content types. Uses fnmatch
        public string ForbiddenContentTypes { get; set; }

        // View width and height in pixels, e.g 800x600. Also accepts 'full'
        public string Viewport { get; set; }

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

        // Whether to enable HTML5 media. Splash default is false, and may cause instability if enabled
        [JsonProperty("html5_media")]
        public bool? Html5Media { get; set; }
    }
}
