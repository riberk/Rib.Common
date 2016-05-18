namespace Rib.Common.Models.Metadata
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ClientEnumAttributeTests
    {
        [TestMethod]
        public void ClientEnumAttributeTest()
        {
            var cea = new ClientEnumAttribute("123");
            Assert.AreEqual("123", cea.FriendlyName);
        }
    }
}