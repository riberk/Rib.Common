using Microsoft.VisualStudio.TestTools.UnitTesting;
using Rib.Common.Application.Web.WebApi;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rib.Common.Application.Web.WebApi
{
    using Moq;
    using Rib.Common.Application.Web.Owin;

    [TestClass]
    public class HttpRequestMessageResolverTests
    {
        private MockRepository _mockRepo;
        private Mock<IOwinContextResolver> _owinResolver;
        private Mock<IWebApiConfigurationFactory> _apiConfig;

        public void Init()
        {
            _mockRepo = new MockRepository(MockBehavior.Strict);
            _owinResolver = _mockRepo.Create<IOwinContextResolver>();
            _apiConfig = _mockRepo.Create<IWebApiConfigurationFactory>();
        }

        [TestMethod]
        public void HttpRequestMessageResolverTest()
        {
            Assert.Fail();
        }
    }
}