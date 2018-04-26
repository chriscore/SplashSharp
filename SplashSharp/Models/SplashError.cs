using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace SplashSharp.Models
{
    public class SplashError
    {
        public string Description { get; set; }
        public int Error { get; set; }
        public HttpStatusCode ErrorStatusCode => (HttpStatusCode)Error;
        public string Type { get; set; }

        //TODO: use more specific error type classes based on value in type
        public ErrorInfo Info { get; set; }
    }
}
