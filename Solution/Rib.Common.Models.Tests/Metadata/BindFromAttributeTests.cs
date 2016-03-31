namespace Rib.Common.Models.Metadata
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class BindFromAttributeTests
    {
        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void BindFromAttributeConstructorNullArgTest()
        {
            new BindFromAttribute("123", null);
        }
    }
}