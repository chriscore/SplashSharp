using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using SplashSharp.Models;

namespace SplashSharp.Tests
{
    [TestClass]
    public class SplashClientVerifyHttpResponseMessageValidTests
    {
        [TestInitialize]
        public void Setup()
        {
            Client = new SplashClient(SplashUrl);
        }

        public SplashClient Client { get; set; }
        public string SplashUrl => "http://localhost:8050/";

        [TestMethod]
        public async Task OK_DoesNotThrow()
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.OK);
            await Client.VerifyHttpResponseMessageValid(responseMessage);
        }

        [TestMethod]
        [ExpectedException(typeof(HttpRequestException))]
        public async Task BadRequest_NoContent_Throws()
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest);
            await Client.VerifyHttpResponseMessageValid(responseMessage);
        }

        [TestMethod]
        [ExpectedException(typeof(SplashWebException))]
        public async Task BadRequest_Content_Throws()
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Post, "http://www.badsite.com/")
            };

            var error = new SplashError
            {
                Description = "The error description",
                Error = 32,
                Info = new ErrorInfo
                {
                    Url = "http://www.badsite.com/",
                    Code = 10,
                    Text = "That didn't work!"
                },
                Type = "Network"
            };

            responseMessage.Content = new StringContent(JsonConvert.SerializeObject(error), Encoding.UTF8, "application/json");
            await Client.VerifyHttpResponseMessageValid(responseMessage);
        }

        [TestMethod]
        public async Task BadRequest_Exception_Correct()
        {
            var responseMessage = new HttpResponseMessage(HttpStatusCode.BadRequest)
            {
                RequestMessage = new HttpRequestMessage(HttpMethod.Post, "http://www.badsite.com/")
            };

            var error = new SplashError
            {
                Description = "The error description",
                Error = 32,
                Info = new ErrorInfo
                {
                    Url = "http://www.badsite.com/",
                    Code = 10,
                    Text = "That didn't work!"
                },
                Type = "Network"
            };

            responseMessage.Content = new StringContent(JsonConvert.SerializeObject(error), Encoding.UTF8, "application/json");

            try
            {
                await Client.VerifyHttpResponseMessageValid(responseMessage);
            }
            catch (SplashWebException e)
            {
                Assert.AreEqual("Request to http://www.badsite.com/ failed with Bad Request: {\"Description\":\"The error description\",\"Error\":32,\"ErrorStatusCode\":32,\"Type\":\"Network\",\"Info\":{\"Type\":null,\"Url\":\"http://www.badsite.com/\",\"Code\":10,\"Text\":\"That didn\'t work!\",\"Error\":null,\"LineNumber\":0,\"Message\":null,\"Source\":null,\"SplashMethod\":null,\"Argument\":null,\"Description\":null}}", e.Message);
                Assert.AreEqual("http://www.badsite.com/", e.RequestUri.ToString());
                Assert.AreEqual(400, e.StatusCode);

                Assert.AreEqual(error.Description, e.SplashError.Description);
                Assert.AreEqual(error.Error, e.SplashError.Error);
                Assert.AreEqual(error.Info.Url, e.SplashError.Info.Url);
                Assert.AreEqual(error.Info.Code, e.SplashError.Info.Code);
                Assert.AreEqual(error.Info.Text, e.SplashError.Info.Text);
                Assert.AreEqual(error.Type, e.SplashError.Type);
            }
        }
    }
}
