namespace Rib.Common.Application.Web.Owin.Modules
{
    using System;
    using System.Threading.Tasks;
    using global::Common.Logging;
    using Microsoft.Owin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class LoggerMiddlewareTests
    {

        [TestMethod]
        public async Task InvokeTest()
        {
            var mr = new MockRepository(MockBehavior.Strict);
            var next = mr.Create<OwinMiddleware>(MockBehavior.Strict, (OwinMiddleware)null);
            var ctx = mr.Create<IOwinContext>();
            var logger = mr.Create<ILog>();
            var request = mr.Create<IOwinRequest>();

            logger.Setup(x => x.Info(It.IsAny<string>())).Verifiable();
            next.Setup(x => x.Invoke(ctx.Object)).Returns(Task.CompletedTask).Verifiable();
            ctx.SetupGet(x => x.Request).Returns(request.Object).Verifiable();
            request.SetupGet(x =>x.Method).Returns("fasefaes").Verifiable();
            request.SetupGet(x =>x.Uri).Returns(new Uri("http://ya.ru")).Verifiable();
            request.SetupGet(x =>x.RemoteIpAddress).Returns("asefasergsdrg").Verifiable();
            request.SetupGet(x =>x.RemotePort).Returns(11).Verifiable();
            var mw = new LoggerMiddleware(next.Object, logger.Object);
            await mw.Invoke(ctx.Object);
            mr.VerifyAll();
        }

        [TestMethod]
        public async Task InvokeErrorTest()
        {
            var mr = new MockRepository(MockBehavior.Strict);
            var next = mr.Create<OwinMiddleware>(MockBehavior.Strict, (OwinMiddleware)null);
            var ctx = mr.Create<IOwinContext>();
            var logger = mr.Create<ILog>();
            var exception = new Exception();
            var request = mr.Create<IOwinRequest>();
            ctx.SetupGet(x => x.Request).Returns(request.Object).Verifiable();
            logger.Setup(x => x.Info(It.IsAny<string>())).Throws(exception);
            request.SetupGet(x => x.Method).Returns("fasefaes").Verifiable();
            request.SetupGet(x => x.Uri).Returns(new Uri("http://ya.ru")).Verifiable();
            request.SetupGet(x => x.RemoteIpAddress).Returns("asefasergsdrg").Verifiable();
            request.SetupGet(x => x.RemotePort).Returns(11).Verifiable();
            var mw = new LoggerMiddleware(next.Object, logger.Object);
            try
            {
                await mw.Invoke(ctx.Object);
            }
            catch (Exception e)
            {
                Assert.AreEqual(exception, e);
                return;
            }
            finally
            {
                mr.VerifyAll();
            }
            Assert.Fail();
        }
    }
}