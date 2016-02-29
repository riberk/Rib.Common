namespace Rib.Common.Models.Exceptions
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RibCommonExceptionTests
    {
        [TestMethod]
        public void RibCommonExceptionTest()
        {
            var me = new RibCommonException();

            Assert.IsNull(me.InnerException);
        }

        [TestMethod]
        public void RibCommonExceptionTest1()
        {
            const string msg = "fgsdgnsdkgh";

            var me = new RibCommonException(msg);

            Assert.IsNull(me.InnerException);
            Assert.AreEqual(msg, me.Message);
        }

        [TestMethod]
        public void RibCommonExceptionTest2()
        {
            const string msg = "fgsdgnsdkgh";
            var ex = new Exception();

            var me = new RibCommonException(msg, ex);

            Assert.AreEqual(ex, me.InnerException);
            Assert.AreEqual(msg, me.Message);
        }
    }
}