using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;

namespace SplashSharp.Requests
{
    public class RenderPngOptions : RenderHtmlOptions
    {
        /// <summary>
        /// Resize the rendered image to the given width in pixels, maintaining aspect ratio
        /// </summary>
        public int? Width { get; set; }

        /// <summary>
        /// Crop the rendered image to the given height in pixels, maintaining aspect ratio
        /// </summary>
        public int? Height { get; set; }

        /// <summary>
        /// When true, extend the viewport to include the whole page. Defaults to false.
        /// If true, requires a non zero wait parameter to be set
        /// </summary>
        public bool? RenderAll { get; set; }

        [JsonIgnore]
        public ScaleMethod? ScaleMethod { get; set; }

        /// <summary>
        /// Whether to use raster or vector based scaling
        /// </summary>
        [JsonProperty("scale_method")]
        public string ScaleMethodInternal => ScaleMethod == null ? null : Enum.GetName(typeof(ScaleMethod), ScaleMethod).ToLower();
    }

    public enum ScaleMethod
    {
        Raster,
        Vector
    }
}
