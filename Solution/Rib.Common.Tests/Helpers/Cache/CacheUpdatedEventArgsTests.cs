namespace Rib.Common.Helpers.Cache
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CacheUpdatedEventArgsTests
    {
        [TestInitialize]
        public void Init()
        {
        }

        [TestMethod]
        public void Constructor() => new CacheUpdatedEventArgs("321");

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument1() => new CacheUpdatedEventArgs(null);

        [TestCleanup]
        public void Clean()
        {
        }

        [TestMethod]
        public void CacheUpdatedEventArgsTest()
        {
            const string fullKey = "123";
            var ea = new CacheUpdatedEventArgs(fullKey);
            Assert.AreEqual(fullKey, ea.FullKey);
        }
    }
}