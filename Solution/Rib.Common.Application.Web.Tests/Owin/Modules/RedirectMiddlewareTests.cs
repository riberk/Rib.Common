namespace Rib.Common.Application.Web.Owin.Modules
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Helpers.CorrelationId;
    using Rib.Common.Models.Helpers;

    [TestClass]
    public class RedirectMiddlewareTests
    {
        [TestMethod]
        public async Task InvokeTest()
        {
            var mr = new MockRepository(MockBehavior.Strict);
            var next = mr.Create<OwinMiddleware>(MockBehavior.Strict, (OwinMiddleware)null);
            var ctx = mr.Create<IOwinContext>();
            var response = mr.Create<IOwinResponse>();

            const string redirectToPage = "http://ya.ru";
            ctx.SetupGet(x => x.Response).Returns(response.Object).Verifiable();
            response.Setup(x => x.Redirect(redirectToPage)).Verifiable();
            var midleware = new RedirectMiddleware(next.Object, redirectToPage);
            await  midleware.Invoke(ctx.Object);

            mr.VerifyAll();
        }
    }
}