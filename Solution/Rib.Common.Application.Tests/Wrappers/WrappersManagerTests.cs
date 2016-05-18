namespace Rib.Common.Application.Wrappers
{
    using System;
    using System.Diagnostics;
    using System.Reflection;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Rib.Common.Helpers.CorrelationId;
    using Rib.Common.Models.Helpers;
    using TsSoft.ContextWrapper;

    [TestClass]
    public class WrappersManagerTests
    {
        private Mock<ICorrelationIdStore> _correlationIdStore;
        private MockRepository _factory;
        private Mock<IItemStore> _itemStore;
        private Mock<IWrapperTypesResolver> _resolver;
        private Mock<IWrappersFactory> _wrappersFactory;

        [TestInitialize]
        public void Init()
        {
            _factory = new MockRepository(MockBehavior.Strict);
            _resolver = _factory.Create<IWrapperTypesResolver>();
            _wrappersFactory = _factory.Create<IWrappersFactory>();
            _correlationIdStore = _factory.Create<ICorrelationIdStore>();
            _itemStore = _factory.Create<IItemStore>();
        }

        [TestMethod]
        public void Constructor()
                =>
                        new WrappersManager(_resolver.Object, _wrappersFactory.Object, _correlationIdStore.Object,
                                            _itemStore.Object);

        [TestMethod]
        public void InitializeAllTest()
        {
            Debug.WriteLine(Assembly.GetExecutingAssembly().Location);
            var manager = Manager();
            _resolver.Setup(x => x.Resolve()).Returns(new[]
            {
                typeof(IItemWrapper<string>),
                typeof(IItemWrapper<Type>),
                typeof(IItemWrapper<CorrelationId>)
            }).Verifiable();
            var stringMock = _factory.Create<IItemWrapper>();
            var typeMock = _factory.Create<IItemWrapper>();
            var corrIdMock = _factory.Create<IItemWrapper<CorrelationId>>();
            _wrappersFactory.Setup(x => x.Get(typeof(IItemWrapper<string>))).Returns(stringMock.Object).Verifiable();
            _wrappersFactory.Setup(x => x.Get(typeof(IItemWrapper<Type>))).Returns(typeMock.Object).Verifiable();
            _wrappersFactory.Setup(x => x.Get(typeof(IItemWrapper<CorrelationId>))).Returns(corrIdMock.Object).Verifiable();
            stringMock.Setup(x => x.Initialize()).Verifiable();
            typeMock.Setup(x => x.Initialize()).Verifiable();
            corrIdMock.Setup(x => x.Initialize()).Verifiable();
            var correlationId = new CorrelationId();
            corrIdMock.Setup(x => x.Current).Returns(correlationId).Verifiable();
            _correlationIdStore.Setup(x => x.Save(correlationId)).Verifiable();
            manager.InitializeAll();
        }

        [TestMethod]
        public void DisposeAllTest()
        {
            var manager = Manager();
            _resolver.Setup(x => x.Resolve()).Returns(new[]
            {
                typeof(IItemWrapper<string>),
                typeof(IItemWrapper<Type>)
            }).Verifiable();
            var stringMock = _factory.Create<IItemWrapper>();
            var typeMock = _factory.Create<IItemWrapper>();
            _wrappersFactory.Setup(x => x.Get(typeof(IItemWrapper<string>))).Returns(stringMock.Object).Verifiable();
            _wrappersFactory.Setup(x => x.Get(typeof(IItemWrapper<Type>))).Returns(typeMock.Object).Verifiable();
            stringMock.Setup(x => x.Dispose()).Verifiable();
            typeMock.Setup(x => x.Dispose()).Verifiable();
            manager.DisposeAll();
        }

        private WrappersManager Manager()
        {
            return new WrappersManager(_resolver.Object, _wrappersFactory.Object, _correlationIdStore.Object, _itemStore.Object);
        }

        [TestMethod]
        public void ClearAllTest()
        {
            _itemStore.Setup(x => x.Clear()).Verifiable();
            Manager().ClearAll();
        }

        [TestCleanup]
        public void Clean()
        {
            _factory.VerifyAll();
        }
    }
}