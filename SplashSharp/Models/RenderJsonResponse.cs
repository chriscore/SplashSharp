using System;
using System.Collections.Generic;
using System.Text;

namespace SplashSharp.Models
{
    public class RenderJsonResponse
    {
        public string Url { get; set; }
        public int[] Geometry { get; set; }
        public string RequestedUrl { get; set; }
        public string Html { get; set; }
        public string Title { get; set; }
        public string Png { get; set; }
        public string Jpeg { get; set; }
        public IList<RenderJsonResponse> ChildFrames { get; set; }
        public string Script { get; set; }
        public IList<string> Console { get; set; }

        // TODO: what type is this?
        public object History { get; set; }

        //TODO: what type is this
        public object Har { get; set; }
    }
}
