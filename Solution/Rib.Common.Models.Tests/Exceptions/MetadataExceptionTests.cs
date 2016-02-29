namespace Rib.Common.Models.Exceptions
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MetadataExceptionTests
    {
        [TestMethod]
        public void MetadataExceptionTest()
        {
            var me = new MetadataException();
            Assert.IsNull(me.InnerException);
        }

        [TestMethod]
        public void MetadataExceptionTest1()
        {
            const string msg = "fgsdgnsdkgh";

            var me = new MetadataException(msg);

            Assert.IsNull(me.InnerException);
            Assert.AreEqual(msg, me.Message);
        }

        [TestMethod]
        public void MetadataExceptionTest2()
        {
            const string msg = "fgsdgnsdkgh";
            var ex = new Exception();

            var me = new MetadataException(msg, ex);

            Assert.AreEqual(ex, me.InnerException);
            Assert.AreEqual(msg, me.Message);
        }
    }
}