namespace Rib.Common.Models.Exceptions
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AttributeExceptionTests
    {
        [TestMethod]
        public void AttributeExceptionTest()
        {
            var ae = new AttributeException(typeof (string));
            Assert.AreEqual(typeof (string), ae.Type);
            Assert.IsNull(ae.InnerException);
        }

        [TestMethod]
        public void AttributeExceptionTest1()
        {
            var exception = new Exception();
            const string message = "fsdgsdg";
            var ae = new AttributeException(message, exception, typeof (string));
            Assert.AreEqual(typeof (string), ae.Type);
            Assert.AreEqual(message, ae.Message);
            Assert.AreEqual(exception, ae.InnerException);
        }

        [TestMethod]
        public void AttributeExceptionTest2()
        {
            const string message = "fsdgsdg";
            var ae = new AttributeException(message, typeof (string));
            Assert.AreEqual(typeof (string), ae.Type);
            Assert.AreEqual(message, ae.Message);
            Assert.IsNull(ae.InnerException);
        }
    }
}