using System;
using System.Linq;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SplashSharp.Tests
{
    public class SplashClientConstructorTestsBase
    {
        public SplashClient Client { get; set; }
        public virtual string SplashUrl { get; set; }
        
        [TestMethod]
        public void CachedArgs_NotNull()
        {
            Assert.IsNotNull(Client.CachedArgs);
        }

        [TestMethod]
        public void CachedArgs_IsEmpty()
        {
            Assert.IsFalse(Client.CachedArgs.Any());
        }

        [TestMethod]
        public void SplashBaseUrl_NotNull()
        {
            Assert.IsNotNull(Client.SplashBaseUrl);
        }

        [TestMethod]
        public void SplashBaseUrl_IsCorrect()
        {
            Assert.AreEqual(new Uri(SplashUrl), Client.SplashBaseUrl);
        }
    }

    [TestClass]
    public class SplashClientTests : SplashClientConstructorTestsBase
    {
        public override string SplashUrl => "http://localhost:8050/";

        public SplashClientTests()
        {
            Client = new SplashClient(SplashUrl);
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

    [TestClass]
    public class SplashClientTestsWithHttpClient : SplashClientConstructorTestsBase
    {
        public override string SplashUrl => "http://localhost:8050/";

        public SplashClientTestsWithHttpClient()
        {
            Client = new SplashClient(SplashUrl, new HttpClient());
        }

        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void InvalidBaseUrl_Throws()
        {
            var client = new SplashClient("not a proper uri", new HttpClient());
        }

        [TestMethod]
        [ExpectedException(typeof(UriFormatException))]
        public void InvalidBaseUrl_MustBeAbsolute()
        {
            var client = new SplashClient("/service", new HttpClient());
        }

        [TestMethod]
        public void InjectedHttpClient_IsSet()
        {
            var http = new HttpClient();
            var client = new SplashClient(SplashUrl, http);
            Assert.AreEqual(http, client.Client);
        }
    }
}
