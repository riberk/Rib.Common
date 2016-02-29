namespace Rib.Common.Models.Exceptions
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class AttributeNotFoundExceptionTests
    {
        [TestMethod]
        public void AttributeNotFoundExceptionTest()
        {
            var attr = typeof (DescriptionAttribute);
            var provider = typeof (string);

            var ae = new AttributeNotFoundException(attr, provider);

            Assert.AreEqual(attr, ae.Type);
            Assert.AreEqual(provider, ae.Provider);
        }

        [TestMethod]
        public void AttributeNotFoundExceptionTest1()
        {
            var attr = typeof (DescriptionAttribute);
            var provider = typeof (string);
            const string msg = "lsdggh";

            var ae = new AttributeNotFoundException(msg, attr, provider);

            Assert.AreEqual(attr, ae.Type);
            Assert.AreEqual(provider, ae.Provider);
            Assert.AreEqual(msg, ae.Message);
        }

        [TestMethod]
        public void AttributeNotFoundExceptionTest2()
        {
            var attr = typeof (DescriptionAttribute);
            var provider = typeof (string);
            const string msg = "lsdggh";
            var ex = new Exception();

            var ae = new AttributeNotFoundException(msg, ex, attr, provider);

            Assert.AreEqual(attr, ae.Type);
            Assert.AreEqual(provider, ae.Provider);
            Assert.AreEqual(msg, ae.Message);
            Assert.AreEqual(ex, ae.InnerException);
        }
    }
}