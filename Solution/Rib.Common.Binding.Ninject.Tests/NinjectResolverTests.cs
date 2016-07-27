namespace Rib.Common.Binding.Ninject
{
    using System;
    using global::Ninject;
    using global::Ninject.Activation;
    using global::Ninject.Parameters;
    using global::Ninject.Planning.Bindings;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class NinjectResolverTests
    {
        private Mock<IKernel> _kernel;
        private MockRepository _mockFactory;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _kernel = _mockFactory.Create<IKernel>();
        }

        private NinjectResolver Create()
        {
            return new NinjectResolver(_kernel.Object);
        }

        [TestMethod]
        public void Constructor() => new NinjectResolver(_kernel.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument1() => new NinjectResolver(null);


        [TestMethod]
        public void GetTest()
        {
            const string expected = "123";
            MockGet(typeof (string), expected, false);
            var res = Create().Get<string>();
            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void GetWithNameTest()
        {
            const string expected = "123";
            MockGetWithName(typeof (string), expected, false, true);
            var res = Create().Get<string>("000");
            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void GetByTypeTest()
        {
            const string expected = "123";
            MockGet(typeof (string), expected, false);
            var res = Create().Get(typeof (string));
            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void GetByTypeWithNameTest()
        {
            const string expected = "123";
            MockGetWithName(typeof (string), expected, false, true);
            var res = Create().Get(typeof (string), "000");
            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void TryGetTest()
        {
            const string expected = "123";
            MockGet(typeof (string), expected, true);
            var res = Create().TryGet<string>();
            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void TryGetWithNameTest()
        {
            const string expected = "123";
            MockGetWithName(typeof (string), expected, true, true);
            var res = Create().TryGet<string>("000");
            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void TryGetByTypeTest()
        {
            const string expected = "123";
            MockGet(typeof (string), expected, true);
            var res = Create().TryGet(typeof (string));
            Assert.AreEqual(expected, res);
        }

        [TestMethod]
        public void TryGetByTypeWithNameTest()
        {
            const string expected = "123";
            MockGetWithName(typeof (string), expected, true, false);
            var res = Create().TryGet(typeof (string), "000");
            Assert.AreEqual(expected, res);
        }


        private void MockGet(Type t, object result, bool optional)
        {
            var request = _mockFactory.Create<IRequest>(MockBehavior.Loose);
            _kernel.Setup(x => x.CreateRequest(t, null, new IParameter[0], optional, true)).Returns(request.Object).Verifiable();
            _kernel.Setup(x => x.Resolve(request.Object)).Returns(new[] {result}).Verifiable();
        }

        private void MockGetWithName(Type t, object result, bool optional, bool uniq)
        {
            var request = _mockFactory.Create<IRequest>(MockBehavior.Loose);
            _kernel
                .Setup(x => x.CreateRequest(t, It.IsAny<Func<IBindingMetadata, bool>>(), new IParameter[0], optional, uniq))
                .Returns(request.Object)
                .Verifiable();
            _kernel.Setup(x => x.Resolve(request.Object)).Returns(new[] {result}).Verifiable();
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}