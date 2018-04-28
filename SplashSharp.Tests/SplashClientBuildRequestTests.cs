using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SplashSharp.Tests
{
    [TestClass]
    public class SplashClientBuildRequestTests
    {
        [TestInitialize]
        public void Setup()
        {
            Client = new SplashClient(SplashUrl);
        }

        public SplashClient Client { get; set; }
        public string SplashUrl => "http://localhost:8050/";

        [TestMethod]
        public void ReturnType_IsHttpRequestMessage()
        {
            var request = Client.BuildSplashRequest("some.endpoint");
            Assert.IsInstanceOfType(request, typeof(HttpRequestMessage));
        }

        [TestMethod]
        public void NullEndpoint_ReturnsInstanceAddress()
        {
            var request = Client.BuildSplashRequest(null);
            Assert.AreEqual(SplashUrl, request.RequestUri.ToString());
        }

        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void InvalidBaseUrl_Throws()
        {
            var client = new SplashClient("not a proper uri");
        }

        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void InvalidBaseUrl_MustBeAbsolute()
        {
            var client = new SplashClient("/service");
        }
    }
}
