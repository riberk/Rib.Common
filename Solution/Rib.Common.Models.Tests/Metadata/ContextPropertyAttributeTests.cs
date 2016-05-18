namespace Rib.Common.Models.Metadata
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ContextPropertyAttributeTests
    {
        [TestMethod]
        public void ContextPropertyAttributeTest()
        {
            var cpa = new ContextPropertyAttribute(typeof(string));
            Assert.AreEqual(typeof(string), cpa.ResolverType);
        }
    }
}