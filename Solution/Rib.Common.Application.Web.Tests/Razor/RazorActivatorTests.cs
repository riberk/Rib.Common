namespace Rib.Common.Application.Web.Razor
{
    using System.Linq;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using RazorEngine.Templating;
    using Rib.Common.DependencyInjection;

    [TestClass]
    public class RazorActivatorTests
    {
        [TestMethod]
        public void CreateInstanceTest()
        {
            var mock = new Mock<IResolver>(MockBehavior.Strict);
            var tMock = new Mock<ITemplate>(MockBehavior.Strict);

            mock.Setup(x => x.Get(typeof(string), null)).Returns(tMock.Object).Verifiable();
            var activator = new RazorActivator(mock.Object);
            var ctx = typeof(InstanceContext).GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance).Single().Invoke(new object[]
            {
                (TypeLoader) null,
                typeof(string)
            }) as InstanceContext;
            var instance = activator.CreateInstance(ctx);

            Assert.AreEqual(instance, tMock.Object);
            mock.VerifyAll();
        }
    }
}