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
        public void ReturnType_IsHttpRequestMessage_Get()
        {
            var request = Client.BuildSplashRequest("some.endpoint");
            Assert.IsInstanceOfType(request, typeof(HttpRequestMessage));
        }

        [TestMethod]
        public void ReturnType_IsHttpRequestMessage_Post()
        {
            var request = Client.BuildSplashRequest("some.endpoint", null);
            Assert.IsInstanceOfType(request, typeof(HttpRequestMessage));
        }

        [TestMethod]
        public void NullEndpoint_ReturnsInstanceAddress_Get()
        {
            var request = Client.BuildSplashRequest(null);
            Assert.AreEqual(SplashUrl, request.RequestUri.ToString());
        }

        [TestMethod]
        public void NullEndpoint_ReturnsInstanceAddress_Post()
        {
            var request = Client.BuildSplashRequest(null, null);
            Assert.AreEqual(SplashUrl, request.RequestUri.ToString());
        }

        [TestMethod]
        public void RequestUri_Correct_Get()
        {
            var request = Client.BuildSplashRequest("some.endpoint");
            Assert.AreEqual("http://localhost:8050/some.endpoint", request.RequestUri.ToString());
        }

        [TestMethod]
        public void RequestUri_Correct_Post()
        {
            var request = Client.BuildSplashRequest("some.endpoint", null);
            Assert.AreEqual("http://localhost:8050/some.endpoint", request.RequestUri.ToString());
        }

        [TestMethod]
        public void RequestVerbCorrect_Get()
        {
            var request = Client.BuildSplashRequest("some.endpoint");
            Assert.AreEqual(HttpMethod.Get, request.Method);
        }

        [TestMethod]
        public void RequestVerbCorrect_Post()
        {
            var request = Client.BuildSplashRequest("some.endpoint", null);
            Assert.AreEqual(HttpMethod.Post, request.Method);
        }
    }
}
