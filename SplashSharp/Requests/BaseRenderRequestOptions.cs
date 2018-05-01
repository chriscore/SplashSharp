using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SplashSharp.Requests
{
    public class BaseRenderRequestOptions
    {
        /// <summary>
        /// The max request timeout in seconds. Defaults to 30, default maximum allowed by splash is 90.
        /// Can be overridden by starting splash with --max-timeout switch
        /// </summary>
        public double? Timeout { get; set; }

        /// <summary>
        /// List of allowed domains. Comma separated list.
        /// </summary>
        public string AllowedDomains { get; set; }

        /// <summary>
        /// Proxy profile name or proxy URL. URLs must have format [protocol://][user:password@]proxyhost[:port]
        /// Proxy URLs must have protocol http or socks5. Port defaults to 1080
        /// </summary>
        public string Proxy { get; set; }

        /// <summary>
        /// Resources to be request-filtered. Comma separated list
        /// </summary>
        public string Filters { get; set; }

        /// <summary>
        /// Stores values in a splash cache. Useful for reducing overhead, such as values for the Js param.
        /// Used with Load_Args
        /// TODO: Exposed in client SavedArgs property, splash references this using an SHA1 hash (the value in the dictionary)
        /// </summary>
        public Dictionary<string, string> SaveArgs { get; set; }

        /// <summary>
        /// A dictionary of &lt;argument_name, SHA1_hash&gt;
        /// TODO: If any arguments are not found, splash responds with HTTP 498. In this case client should repeat the request, but use save_args and send full argument values
        /// Splash uses LRU cache to store values; the number of entries is limited, and cache is cleared after each Splash restart
        /// </summary>
        public Dictionary<string, string> LoadArgs { get; set; }
    }
}
