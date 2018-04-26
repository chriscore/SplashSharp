using System;
using System.Collections.Generic;
using System.Text;

namespace SplashSharp.Requests
{
    public class RenderHarOptions : RenderHtmlOptions
    {
        /// <summary>
        /// Include response content in HAR records. Defaults to false
        /// </summary>
        public bool? ResponseBody { get; set; }
    }
}
