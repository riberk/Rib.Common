namespace Rib.Common.Application.Web.Owin.Modules
{
    using System;
    using System.Threading.Tasks;
    using global::Common.Logging;
    using Microsoft.Owin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class ExceptionHandlerMiddlewareTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullArgTest() => new ExceptionHandlerMiddleware(null, null);

        [TestMethod]
        public async Task InvokeTest()
        {
            var mr = new MockRepository(MockBehavior.Strict);
            var next = mr.Create<OwinMiddleware>(MockBehavior.Strict, (OwinMiddleware) null);
            var ctx = mr.Create<IOwinContext>();
            var logger = mr.Create<ILog>();
            var exception = new Exception();

            logger.Setup(x => x.Fatal(It.IsAny<object>(), exception)).Verifiable();
            next.Setup(x => x.Invoke(ctx.Object)).Returns(async () =>
            {
                await Task.Delay(1);
                throw exception;
            }).Verifiable();

            var mw = new ExceptionHandlerMiddleware(next.Object, logger.Object);
            try
            {
                await mw.Invoke(ctx.Object);
            }
            catch (Exception e)
            {
                Assert.AreEqual(exception, e);
            }

            mr.VerifyAll();
        }

        [TestMethod]
        public async Task InvokeNullTest()
        {
            var mr = new MockRepository(MockBehavior.Strict);
            var next = mr.Create<OwinMiddleware>(MockBehavior.Strict, (OwinMiddleware) null);
            var ctx = mr.Create<IOwinContext>();
            var logger = mr.Create<ILog>();

            next.Setup(x => x.Invoke(ctx.Object)).Returns(() => null).Verifiable();

            var mw = new ExceptionHandlerMiddleware(next.Object, logger.Object);
            await mw.Invoke(ctx.Object);

            mr.VerifyAll();
        }
    }
}