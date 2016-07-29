namespace Rib.Common.Application.Web.Owin.Modules
{
    using System;
    using System.Runtime.Remoting.Messaging;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class SetOwinContextToCallContextMiddlewareTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullArgTest() => new SetOwinContextToCallContextMiddleware(null, null);

        [TestMethod]
        public async Task InvokeTest()
        {
            const string key = "123fgefg";
            try
            {
                var next = new Mock<OwinMiddleware>(MockBehavior.Strict, (OwinMiddleware)null);
                var ctx = new Mock<IOwinContext>(MockBehavior.Strict);

                var midlware = new SetOwinContextToCallContextMiddleware(next.Object, key);
                next.Setup(x => x.Invoke(ctx.Object)).Returns((IOwinContext c) =>
                {
                    Assert.AreEqual(ctx.Object, CallContext.LogicalGetData(key));
                    return Task.CompletedTask;
                }).Verifiable();

                await midlware.Invoke(ctx.Object);

                next.VerifyAll();
                ctx.VerifyAll();
            }
            finally
            {
                CallContext.LogicalSetData(key, null);
            }

        }
    }
}