namespace Rib.Common.Helpers.Cache
{
    using System.Runtime.Caching;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class StubCachePolicyFactoryTests
    {
        [TestMethod]
        public void CreateTest()
        {
            var factory = new StubCachePolicyFactory();
            var defaultItemPolicy = new CacheItemPolicy();
            var actual = factory.Create<string>();
            Assert.AreEqual(defaultItemPolicy.AbsoluteExpiration, actual.AbsoluteExpiration);
            Assert.AreEqual(defaultItemPolicy.ChangeMonitors.Count, actual.ChangeMonitors.Count);
            Assert.AreEqual(defaultItemPolicy.Priority, actual.Priority);
            Assert.AreEqual(defaultItemPolicy.RemovedCallback, actual.RemovedCallback);
            Assert.AreEqual(defaultItemPolicy.SlidingExpiration, actual.SlidingExpiration);
            Assert.AreEqual(defaultItemPolicy.UpdateCallback, actual.UpdateCallback);
        }
    }
}