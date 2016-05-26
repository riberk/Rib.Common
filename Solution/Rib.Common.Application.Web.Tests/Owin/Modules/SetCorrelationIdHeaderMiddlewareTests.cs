namespace Rib.Common.Application.Web.Owin.Modules
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Application.Web.WebApi.Helpers;
    using Rib.Common.Helpers.CorrelationId;
    using Rib.Common.Models.Helpers;

    [TestClass]
    public class SetCorrelationIdHeaderMiddlewareTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullArgTest() => new SetCorrelationIdHeaderMiddleware(null, null); 

        [TestMethod]
        public async Task InvokeTest()
        {
            var mr = new MockRepository(MockBehavior.Strict);
            var next = mr.Create<OwinMiddleware>(MockBehavior.Strict, (OwinMiddleware)null);
            var ctx = mr.Create<IOwinContext>();
            var cidStore = mr.Create<ICorrelationIdStore>();
            var response = mr.Create<IOwinResponse>();
            var hDict = new HeaderDictionary(new Dictionary<string, string[]>());
            var correlationId = new CorrelationId();

            next.Setup(x => x.Invoke(ctx.Object)).Returns(Task.CompletedTask).Verifiable();
            ctx.SetupGet(x => x.Response).Returns(response.Object).Verifiable();
            response.SetupGet(x => x.Headers).Returns(hDict).Verifiable();
            cidStore.Setup(x => x.Read()).Returns(correlationId).Verifiable();

            Assert.AreEqual(0, hDict.Count);

            var midleware = new SetCorrelationIdHeaderMiddleware(next.Object, cidStore.Object);
            await midleware.Invoke(ctx.Object);

            Assert.AreEqual(1, hDict.Count);
            var actual = hDict["X-CORRELATIONID"];
            Assert.AreEqual(correlationId.ToString(), actual);
            mr.VerifyAll();
        }
    }
}