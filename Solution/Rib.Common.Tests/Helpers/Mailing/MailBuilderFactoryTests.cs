namespace Rib.Common.Helpers.Mailing
{
    using System;
    using Rib.Common.Ninject;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.DependencyInjection;

    [TestClass]
    public class MailBuilderFactoryTests
    {
        private MockRepository _mockFactory;
        private Mock<IResolver> _resolver;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _resolver = _mockFactory.Create<IResolver>();
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorArgumentException() => new MailBuilderFactory(null);

        [TestMethod]
        public void CreateTest()
        {
            var mbMock = _mockFactory.Create<IMailBuilder>();
            _resolver.Setup(x => x.Get<IMailBuilder>(null)).Returns(mbMock.Object).Verifiable();
            Assert.AreEqual(mbMock.Object, new MailBuilderFactory(_resolver.Object).Create());
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}