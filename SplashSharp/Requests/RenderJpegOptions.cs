using System;
using System.Collections.Generic;
using System.Text;

namespace SplashSharp.Requests
{
    public class RenderJpegOptions : RenderPngOptions
    {
        /// <summary>
        /// An int from 0 to 100. Quality values above 95 should be avoided
        /// Defaults to 75
        /// </summary>
        public int? Quality { get; set; }
    }
}
