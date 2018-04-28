using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using SplashSharp.Serialization;

namespace SplashSharp.Tests
{
    [TestClass]
    public class SplashClientSerializerTests
    {
        [TestMethod]
        public void Singleton_NotNull()
        {
            var serializer = SplashClient.SplashJsonSerializerSettings;
            Assert.IsNotNull(serializer);
        }

        [TestMethod]
        public void ContractResolver_UsesSnakeCase()
        {
            var resolver = SplashClient.SplashJsonSerializerSettings.ContractResolver as DefaultContractResolver;
            Assert.IsNotNull(resolver);

            Assert.IsInstanceOfType(resolver.NamingStrategy, typeof(SnakeCaseNamingStrategy));
        }

        [TestMethod]
        public void Uses_NullableBoolJsonConverter()
        {
            var serializer = SplashClient.SplashJsonSerializerSettings;
            Assert.IsTrue(serializer.Converters.SingleOrDefault(c => c.GetType() == typeof(NullableBoolJsonConverter)) != null);
        }

        [TestMethod]
        public void Serializes_NullableBoolToBit()
        {
            var expected = "{\"true\":1,\"false\":0}";
            var obj = new
            {
                @true = true as bool?,
                @false = false as bool?,
                @null = null as bool?
            };

            var actual = JsonConvert.SerializeObject(obj, SplashClient.SplashJsonSerializerSettings);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Serializes_PropertyNamesToSnakeCase()
        {
            var expected = "{\"a_property_name\":\"someValue\"}";
            var obj = new
            {
                APropertyName = "someValue"
            };

            var actual = JsonConvert.SerializeObject(obj, SplashClient.SplashJsonSerializerSettings);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void Ignores_NullValues()
        {
            var expected = "{\"a_property_name\":\"someValue\"}";
            var obj = new
            {
                APropertyName = "someValue",
                ANullProperty = null as object
            };

            var actual = JsonConvert.SerializeObject(obj, SplashClient.SplashJsonSerializerSettings);
            Assert.AreEqual(expected, actual);
        }
    }
}
