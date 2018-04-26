using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SplashSharp.Models;

namespace SplashSharp
{
    /// <summary>
    /// An exception representing a non HTTP 200 response
    /// </summary>
    public class SplashWebException : Exception
    {
        public SplashWebException(HttpResponseMessage response, string responseContent)
            : base($"Request to {response.RequestMessage.RequestUri} failed with {response.ReasonPhrase}")
        {
            RequestUri = response.RequestMessage.RequestUri;
            StatusCode = (int)response.StatusCode;
            ReasonPhrase = response.ReasonPhrase;
            ResponseContent = responseContent;

            try
            {
                SplashError = JsonConvert.DeserializeObject<SplashError>(responseContent, SplashClient.SplashJsonSerializerSettings);
            }
            catch(Exception)
            { }
        }

        public Uri RequestUri { get; set; }
        public int StatusCode { get; set; }
        public string ReasonPhrase { get; set; }
        public string ResponseContent { get; set; }

        //TODO: use more specific error type classes based on responseContent.type pattern matched
        public SplashError SplashError { get; set; }
    }
}
