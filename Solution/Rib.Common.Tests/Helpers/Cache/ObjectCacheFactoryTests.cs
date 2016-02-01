namespace Rib.Common.Helpers.Cache
{
    using System.Runtime.Caching;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ObjectCacheFactoryTests
    {
        [TestMethod]
        public void CreateOneInstanceTest()
        {
            var factory = new ObjectCacheFactory();
            Assert.AreEqual(factory.Create(), factory.Create());
        }

        [TestMethod]
        public void CreateDefaultMemoryCache()
        {
            var factory = new ObjectCacheFactory();
            Assert.AreEqual(MemoryCache.Default, factory.Create());
        }
    }
}