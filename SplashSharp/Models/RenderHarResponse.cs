using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace SplashSharp.Models
{
    public class RenderHarResponse
    {
        public string Raw { get; set; }
        public Log Log { get; set; }
    }

    public class ErrorInfo
    {
        //TODO: split these into separate error types based on errorResponse.type pattern matched

        // Common properties
        public string Type { get; set; }

        // Properties for 'Network' type 
        public string Url { get; set; }
        public int Code { get; set; }
        public string Text { get; set; }
        
        // Properties for ScriptError type
        public string Error { get; set; }
        public int LineNumber { get; set; }
        public string Message { get; set; }
        public string Source { get; set; }
    }

    public class Log
    {
        public Page[] Pages { get; set; }
        public string Version { get; set; }
        public Entry[] Entries { get; set; }
        public Creator Creator { get; set; }
        public Browser Browser { get; set; }
    }

    public class Creator
    {
        public string Version { get; set; }
        public string Name { get; set; }
    }

    public class Browser
    {
        public string Version { get; set; }
        public string Comment { get; set; }
        public string Name { get; set; }
    }

    public class Page
    {
        public Pagetimings PageTimings { get; set; }
        public string Id { get; set; }
        public string Title { get; set; }
        public DateTime StartedDateTime { get; set; }
    }

    public class Pagetimings
    {
        [JsonProperty("_onStarted")]
        public int OnStarted { get; set; }
        [JsonProperty("_onPrepareStart")]
        public int OnPrepareStart { get; set; }
        public int OnLoad { get; set; }
        public int OnContentLoad { get; set; }
    }

    public class Entry
    {
        [JsonProperty("_splash_processing_state")]
        public string SplashProcessingState { get; set; }
        public Response Response { get; set; }
        public int Time { get; set; }
        public Request Request { get; set; }
        public DateTime StartedDateTime { get; set; }
        public string Pageref { get; set; }
        public Cache Cache { get; set; }
        public Timings Timings { get; set; }
    }

    public class Response
    {
        public Cookie[] Cookies { get; set; }
        public NameValuePair[] Headers { get; set; }
        public bool Ok { get; set; }
        public string RedirectUrl { get; set; }
        public int BodySize { get; set; }
        public int Status { get; set; }
        public Content Content { get; set; }
        public string StatusText { get; set; }
        public string Url { get; set; }
        public int HeadersSize { get; set; }
        public string HttpVersion { get; set; }
    }

    public class Content
    {
        public string MimeType { get; set; }
        public int Size { get; set; }
    }

    public class Cookie
    {
        public string Value { get; set; }
        public bool Secure { get; set; }
        public bool HttpOnly { get; set; }
        public string Name { get; set; }
        public DateTime Expires { get; set; }
        public string Domain { get; set; }
        public string Path { get; set; }
    }

    public class NameValuePair
    {
        public string Value { get; set; }
        public string Name { get; set; }
    }

    public class Request
    {
        public Cookie[] Cookies { get; set; }
        public NameValuePair[] Headers { get; set; }
        public string Url { get; set; }
        public int BodySize { get; set; }
        public object[] QueryString { get; set; }
        public string Method { get; set; }
        public int HeadersSize { get; set; }
        public string HttpVersion { get; set; }
    }

    public class Cache
    {
    }

    public class Timings
    {
        public int Wait { get; set; }
        public int Ssl { get; set; }
        public int Connect { get; set; }
        public int Dns { get; set; }
        public int Blocked { get; set; }
        public int Receive { get; set; }
        public int Send { get; set; }
    }

}
