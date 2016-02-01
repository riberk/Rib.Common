namespace Rib.Common.Helpers.Mailing
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MailMessageFactoryTests
    {
        [TestMethod]
        public void CreateTest()
        {
            var msg = new MailMessageFactory().Create();
            Assert.IsTrue(msg.IsBodyHtml);
        }
    }
}