namespace Rib.Common.Models.Contract
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class PagedResponseTests
    {
        [TestMethod]
        public void PagedResponseNullTest()
        {
            var pr = new PagedResponse<string>(null, 10);

            Assert.IsNotNull(pr.Value);
            Assert.AreEqual(0, pr.Value.Count);
            Assert.AreEqual(10, pr.Count);
        }

        [TestMethod]
        public void PagedResponseTest()
        {
            var strings = new[] {"123", "321"};
            var pr = new PagedResponse<string>(strings, 10);

            Assert.AreEqual(strings, pr.Value);
            Assert.AreEqual(10, pr.Count);
        }
    }
}