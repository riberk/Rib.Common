namespace Rib.Common.Helpers.Mailing
{
    using System;
    using System.IO;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using Rib.Common.Helpers.Tmp;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class MailSenderTests
    {
        private MockRepository _mockFactory;
        private Mock<ISmtpClientFactory> _smtpFactory;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _smtpFactory = _mockFactory.Create<ISmtpClientFactory>();
        }

        [TestMethod]
        public void Constructor() => new MailSender(_smtpFactory.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument() => new MailSender(null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task SendAsyncNullArgument() => await new MailSender(_smtpFactory.Object).SendAsync(null);

        [TestMethod]
        public async Task SendAsyncTest()
        {
            using (var tmpFolder = TmpFolder.Create("C:\\tmp"))
            {
                var files = Directory.GetFiles(tmpFolder.Path, "*.eml");
                Assert.AreEqual(0, files.Length);
                var smtpClient = new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                    PickupDirectoryLocation = tmpFolder.Path,
                    EnableSsl = false
                };
                var mailMessage = new MailMessage
                {
                    From = new MailAddress("pavel@mmm.from")
                };
                mailMessage.To.Add("pavel@mmm.to");
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = "<a>Link!</a>";
                mailMessage.Subject = "Subject";
                _smtpFactory.Setup(x => x.Create()).Returns(smtpClient).Verifiable();
                await new MailSender(_smtpFactory.Object).SendAsync(mailMessage);
                files = Directory.GetFiles(tmpFolder.Path, "*.eml");
                Assert.AreEqual(1, files.Length);
            }
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}