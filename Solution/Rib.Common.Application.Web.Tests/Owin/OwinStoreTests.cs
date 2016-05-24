namespace Rib.Common.Application.Web.Owin
{
    using System;
    using System.Runtime.Remoting.Messaging;
    using JetBrains.Annotations;
    using Microsoft.Owin;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using TsSoft.ContextWrapper;

    [TestClass]
    public class OwinStoreTests
    {
        [NotNull] private Mock<IOwinContextResolver> _owinCtxResolver;
        [NotNull] private MockRepository _mockRepo;
        [NotNull] private Mock<IOwinContext> _owinCtx;

        [TestInitialize]
        public void Initialize()
        {
            _mockRepo = new MockRepository(MockBehavior.Strict);
            _owinCtxResolver =_mockRepo.Create<IOwinContextResolver>();
            _owinCtx = _mockRepo.Create<IOwinContext>();
        }

        internal OwinStore Create() => new OwinStore(_owinCtxResolver.Object);

        [TestMethod]
        public void GetTestWithOwin()
        {
            const string key = "sr;rklngsdlksd";
            const string value = "ldfgnposdftjhiopsdrgjkpsd";

            _owinCtxResolver.SetupGet(x => x.Current).Returns(_owinCtx.Object).Verifiable();
            _owinCtx.Setup(x => x.Get<ILazy<string>>(key)).Returns(new L<string>(value)).Verifiable();

            var res = Create().Get<string>(key);
            Assert.AreEqual(value, res);
        }

        [TestMethod]
        public void GetTestCallContext()
        {
            const string key = "sr;rklngsdlksd";
            const string value = "ldfgnposdftjhiopsdrgjkpsd";
            try
            {
                CallContext.LogicalSetData(key, value);
                _owinCtxResolver.SetupGet(x => x.Current).Returns((IOwinContext)null).Verifiable();
                var res = Create().Get<string>(key);
                Assert.AreEqual(value, res);
            }
            finally
            {
                CallContext.LogicalSetData(key, null);
            }
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void GetNullArgTest() => Create().Get<string>(null);

        [TestMethod]
        public void NewContextTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void DisposeIfCreatedTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void DisposeAllTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void ClearTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void IsInitializedTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void IsValueCreatedTest()
        {
            Assert.Fail();
        }

        [TestMethod]
        public void DisposeTest()
        {
            Assert.Fail();
        }

        [TestCleanup]
        public void Clean()
        {
            _mockRepo.VerifyAll();
        }

        private class L<T> : ILazy<T>
        {
            public L(T value)
            {
                Value = value;
                IsValueCreated = true;
            }
            /// <summary>Вычисленное значение</summary>
            public T Value { get; }

            /// <summary>Значение вычислено</summary>
            public bool IsValueCreated { get; }
        }
    }
}