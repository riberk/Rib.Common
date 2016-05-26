namespace Rib.Common.Application.Web.Owin.Modules
{
    using System;
    using System.Threading.Tasks;
    using global::Common.Logging;
    using Microsoft.Owin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Application.Wrappers;

    [TestClass]
    public class InitializeWrappersMiddlewareTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void NullArgTest() => new InitializeWrappersMiddleware(null, null);

        [TestMethod]
        public async Task InvokeTest()
        {
            var mr = new MockRepository(MockBehavior.Strict);
            var next = mr.Create<OwinMiddleware>(MockBehavior.Strict, (OwinMiddleware)null);
            var ctx = mr.Create<IOwinContext>();
            var wrappersManager = mr.Create<IWrappersManager>();

            next.Setup(x => x.Invoke(ctx.Object)).Returns(Task.CompletedTask).Verifiable();
            wrappersManager.Setup(x => x.InitializeAll()).Verifiable();
            wrappersManager.Setup(x => x.DisposeAll()).Verifiable();
            var mw = new InitializeWrappersMiddleware(next.Object, wrappersManager.Object);
            await mw.Invoke(ctx.Object);
            mr.VerifyAll();
        }
    }
}