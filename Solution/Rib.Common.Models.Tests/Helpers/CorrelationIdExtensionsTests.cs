namespace Rib.Common.Models.Helpers
{
    using System;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class CorrelationIdExtensionsTests
    {
        [TestMethod]
        public void ToDatedStringTest()
        {
            var id = Guid.NewGuid();
            var dt = DateTime.UtcNow;
            var ds = new CorrelationId(id).ToDatedString();

            Assert.AreEqual($"{id}__{dt.ToString("yyyyMMdd_hhmmss")}", ds);
        }
    }
}