namespace Rib.Common.Application.Wrappers
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.DependencyInjection;
    using TsSoft.ContextWrapper;

    [TestClass]
    public class WrappersFactoryTests
    {
        private MockRepository _factory;
        private Mock<IResolver> _resolver;

        [TestInitialize]
        public void Init()
        {
            _factory = new MockRepository(MockBehavior.Strict);
            _resolver = _factory.Create<IResolver>();
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ConstructorArgumentException() => new WrappersFactory(null);

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetArgumentException() => new WrappersFactory(_resolver.Object).Get(null);

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void GetWithNullResultExceptionTest()
        {
            _resolver.Setup(x => x.TryGet(typeof(string), null)).Returns(null).Verifiable();
            var wFactory = new WrappersFactory(_resolver.Object);
            wFactory.Get(typeof(string));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void GetWithNotItemWrapperResultExceptionTest()
        {
            _resolver.Setup(x => x.TryGet(typeof(string), null)).Returns("123").Verifiable();
            var wFactory = new WrappersFactory(_resolver.Object);
            wFactory.Get(typeof(string));
        }

        [TestMethod]
        public void GetTest()
        {
            var wrapperMock = _factory.Create<IItemWrapper>();
            _resolver.Setup(x => x.TryGet(typeof(string), null)).Returns(wrapperMock.Object).Verifiable();
            var wFactory = new WrappersFactory(_resolver.Object);
            var actual = wFactory.Get(typeof(string));
            Assert.AreEqual(wrapperMock.Object, actual);
        }

        [TestCleanup]
        public void Clean()
        {
            _factory.VerifyAll();
        }
    }
}