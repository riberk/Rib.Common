namespace Rib.Common.Helpers.Cache
{
    using System;
    using System.Linq;
    using System.Runtime.Caching;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class CacheClearerTests
    {
        private MockRepository _mockFactory;
        private Mock<IObjectCacheFactory> _objectCacheFactory;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _objectCacheFactory = _mockFactory.Create<IObjectCacheFactory>();
        }

        [NotNull]
        private string FullKey(string prefix, string key)
        {
            return $"{prefix}|{key}";
        }

        [TestMethod]
        public void ClearAllCachesTest()
        {
            const string prefix = "123123af";
            var fk1 = FullKey(prefix, "111");
            var fk2 = FullKey(prefix, "222");
            var memoryCache = new MemoryCache("sdgkmnsdlkghn");
            _objectCacheFactory.Setup(x => x.Create()).Returns(memoryCache).Verifiable();
            Assert.AreEqual(0, memoryCache.GetCount());
            memoryCache.Add(fk1, new MemoryCacherTests.One(), new CacheItemPolicy());
            memoryCache.Add(fk2, new MemoryCacherTests.One(), new CacheItemPolicy());
            Assert.AreEqual(2, memoryCache.Count());
            new CacheCleaner(_objectCacheFactory.Object).Clean();
            Assert.AreEqual(0, memoryCache.GetCount());
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClearByKeyNullArg() => new CacheCleaner(_objectCacheFactory.Object).Clean(null);

        [TestMethod]
        public void ClearByKey()
        {
            const string prefix = "123123af";
            var fk1 = FullKey(prefix, "111");
            var value1 = new MemoryCacherTests.One();

            var fk2 = FullKey(prefix, "222");
            var value2 = new MemoryCacherTests.One();

            var memoryCache = new MemoryCache("sdgkmnsdlkghn");
            _objectCacheFactory.Setup(x => x.Create()).Returns(memoryCache).Verifiable();
            Assert.AreEqual(0, memoryCache.GetCount());

            memoryCache.Add(fk1, value1, new CacheItemPolicy());
            memoryCache.Add(fk2, value2, new CacheItemPolicy());

            Assert.AreEqual(2, memoryCache.Count());
            new CacheCleaner(_objectCacheFactory.Object).Clean(fk1);
            Assert.AreEqual(1, memoryCache.GetCount());
            Assert.IsNotNull(memoryCache[fk2]);
            Assert.AreEqual(value2, memoryCache[fk2]);
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}