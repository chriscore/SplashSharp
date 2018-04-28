using System.Linq;
using System.Net.Http;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace SplashSharp.Tests
{
    public class SplashClientConstructorTestsBase
    {
        public SplashClient Client { get; set; }
        public virtual string SplashUrl { get; }
        
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
            Assert.AreEqual(SplashUrl, Client.SplashBaseUrl);
        }
    }

    [TestClass]
    public class SplashClientConstructorTests : SplashClientConstructorTestsBase
    {
        public override string SplashUrl => "http://localhost:8050";

        public SplashClientConstructorTests()
        {
            Client = new SplashClient(SplashUrl);
        }
    }

    [TestClass]
    public class SplashClientConstructorWithClientTests : SplashClientConstructorTestsBase
    {
        public override string SplashUrl => "http://localhost:8050";

        public SplashClientConstructorWithClientTests()
        {
            Client = new SplashClient(SplashUrl, new HttpClient());
        }
    }
}
