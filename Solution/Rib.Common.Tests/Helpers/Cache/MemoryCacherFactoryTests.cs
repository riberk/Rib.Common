namespace Rib.Common.Helpers.Cache
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class MemoryCacherFactoryTests
    {
        private MockRepository _mockFactory;
        private Mock<IObjectCacheFactory> _objCacheFactory;
        private Mock<ICachePolicyFactory> _policyFactory;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _policyFactory = _mockFactory.Create<ICachePolicyFactory>();
            _objCacheFactory = _mockFactory.Create<IObjectCacheFactory>();

            new MemoryCacherFactory(_policyFactory.Object, _objCacheFactory.Object);
        }

        [TestMethod]
        public void Constructor() => new MemoryCacherFactory(_policyFactory.Object, _objCacheFactory.Object);

        [TestMethod]
        public void CreateTest()
        {
            var res = new MemoryCacherFactory(_policyFactory.Object, _objCacheFactory.Object).Create<string>();
            Assert.IsNotNull(res);
            Assert.AreEqual(typeof (MemoryCacher<string>), res.GetType());
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}