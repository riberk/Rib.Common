namespace Rib.Common.Helpers
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ObjectExtensionsTests
    {
        [TestMethod]
        [ExpectedException(typeof (NullReferenceException))]
        public void ThrowIfNullWithNullTest()
        {
            object o = null;
            o.ThrowIfNull("name");
        }

        [TestMethod]
        public void ThrowIfNullNotNullTest()
        {
            var o = new object();
            Assert.AreEqual(o, o.ThrowIfNull("123"));
        }
    }
}