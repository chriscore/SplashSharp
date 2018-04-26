using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace SplashSharp.Models
{
    public class SplashResponseWrapper<T>
    {
        public SplashResponseWrapper(HttpResponseMessage response)
        {
            RawResponse = response;
        }

        public HttpResponseMessage RawResponse { get; set; }
        public T Data { get; set; }
    }
}
