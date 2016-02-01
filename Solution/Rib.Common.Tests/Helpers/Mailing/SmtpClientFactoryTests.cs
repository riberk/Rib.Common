namespace Rib.Common.Helpers.Mailing
{
    using System.Net.Mail;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SmtpClientFactoryTests
    {
        [TestMethod]
        public void CreateTest()
        {
            using (var smtpClient = new SmtpClientFactory().Create())
            {
                Assert.AreEqual(SmtpDeliveryMethod.Network, smtpClient.DeliveryMethod);
                Assert.AreEqual(false, smtpClient.UseDefaultCredentials);
                Assert.AreEqual("smtp.gmail.com", smtpClient.Host);
                Assert.AreEqual(587, smtpClient.Port);
            }
        }
    }
}