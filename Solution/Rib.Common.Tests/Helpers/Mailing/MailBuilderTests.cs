namespace Rib.Common.Helpers.Mailing
{
    using System;
    using System.IO;
    using System.Net.Mail;
    using System.Threading.Tasks;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class MailBuilderTests
    {
        private Mock<IMailMessageFactory> _mailMessageFactory;
        private Mock<IMailSender> _mailSender;
        private MockRepository _mockFactory;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _mailSender = _mockFactory.Create<IMailSender>();
            _mailMessageFactory = _mockFactory.Create<IMailMessageFactory>();
        }

        [NotNull]
        private MailBuilder GetBuilder()
        {
            _mailMessageFactory.Setup(x => x.Create()).Returns(new MailMessage()).Verifiable();
            return new MailBuilder(_mailSender.Object, _mailMessageFactory.Object);
        }

        
        [TestMethod]
        public void BodyTest()
        {
            var builder = GetBuilder();
            var body = "123";
            builder.Body(body);
            var message = builder.ToMessage();
            Assert.AreEqual(body, message.Body);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void BodyArgumentNullTest()
        {
            GetBuilder().Body(null);
        }

        [TestMethod]
        public void FromTest()
        {
            const string from = "from@mail.ru";
            Assert.AreEqual(new MailAddress(from), GetBuilder().From(from).ToMessage().From);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void FromArgumentNullTest()
        {
            GetBuilder().From((string) null);
        }

        [TestMethod]
        public void FromTest1()
        {
            const string from = "from@mail.ru";
            Assert.AreEqual(new MailAddress(from), GetBuilder().From(new MailAddress(from)).ToMessage().From);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void FromArgumentNullTest1()
        {
            GetBuilder().From((MailAddress) null);
        }

        [TestMethod]
        public void AddToTest()
        {
            const string to = "to@mail.ru";
            var mailMessage = GetBuilder().AddTo(to).ToMessage();
            Assert.AreEqual(1, mailMessage.To.Count);
            Assert.AreEqual(new MailAddress(to), mailMessage.To[0]);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void AddToArgumentNullTest()
        {
            GetBuilder().AddTo((string) null).ToMessage();
        }

        [TestMethod]
        public void AddToTest1()
        {
            const string to = "to@mail.ru";
            const string to1 = "to1@mail.ru";
            var mailMessage = GetBuilder().AddTo(to, to1).ToMessage();
            Assert.AreEqual(2, mailMessage.To.Count);
            Assert.AreEqual(new MailAddress(to), mailMessage.To[0]);
            Assert.AreEqual(new MailAddress(to1), mailMessage.To[1]);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void AddToArgumentNullTest1()
        {
            GetBuilder().AddTo((string[]) null).ToMessage();
        }

        [TestMethod]
        public void AddToTest2()
        {
            const string to = "to@mail.ru";
            var mailMessage = GetBuilder().AddTo(new MailAddress(to)).ToMessage();
            Assert.AreEqual(1, mailMessage.To.Count);
            Assert.AreEqual(new MailAddress(to), mailMessage.To[0]);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void AddToArgumentNullTest2()
        {
            GetBuilder().AddTo((MailAddress) null).ToMessage();
        }

        [TestMethod]
        public void AddToTest3()
        {
            const string to = "to@mail.ru";
            const string to1 = "to1@mail.ru";
            var mailMessage = GetBuilder().AddTo(new MailAddress(to), new MailAddress(to1)).ToMessage();
            Assert.AreEqual(2, mailMessage.To.Count);
            Assert.AreEqual(new MailAddress(to), mailMessage.To[0]);
            Assert.AreEqual(new MailAddress(to1), mailMessage.To[1]);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void AddToArgumentNullTest3()
        {
            GetBuilder().AddTo((MailAddress[]) null).ToMessage();
        }

        [TestMethod]
        public void SubjectTest()
        {
            const string subject = "from@mail.ru";
            Assert.AreEqual(subject, GetBuilder().Subject(subject).ToMessage().Subject);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void SubjectNullArgumentTest()
        {
            GetBuilder().Subject(null);
        }

        [TestMethod]
        public void AddAttachmentTest()
        {
            using (var ms1 = new MemoryStream())
            {
                const string name = "123";
                var attachment = new Attachment(ms1, name);
                var msg = GetBuilder().AddAttachment(attachment).ToMessage();
                Assert.AreEqual(1, msg.Attachments.Count);
                Assert.AreEqual(ms1, msg.Attachments[0].ContentStream);
                Assert.AreEqual(name, msg.Attachments[0].Name);
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void AddAttachmentNullArgumentTest()
        {
            GetBuilder().AddAttachment(null);
        }

        [TestMethod]
        public void AddAttachmentsTest()
        {
            using (var ms1 = new MemoryStream())
            using (var ms2 = new MemoryStream())
            {
                const string name1 = "123";
                const string name2 = "12321";
                var msg = GetBuilder().AddAttachments(new Attachment(ms1, name1), new Attachment(ms2, name2)).ToMessage();

                Assert.AreEqual(2, msg.Attachments.Count);

                Assert.AreEqual(ms1, msg.Attachments[0].ContentStream);
                Assert.AreEqual(name1, msg.Attachments[0].Name);

                Assert.AreEqual(ms2, msg.Attachments[1].ContentStream);
                Assert.AreEqual(name2, msg.Attachments[1].Name);
            }
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void AddAttachmentsNullArgumentTest()
        {
            GetBuilder().AddAttachments(null);
        }

        [TestMethod]
        public async Task SendAsyncTest()
        {
            var msg = new MailMessage();
            _mailSender.Setup(x => x.SendAsync(msg)).Returns(Task.FromResult(0)).Verifiable();
            _mailMessageFactory.Setup(x => x.Create()).Returns(msg).Verifiable();
            var builder = new MailBuilder(_mailSender.Object, _mailMessageFactory.Object);
            await builder.SendAsync();
        }

        [TestCleanup]
        public void Cleanup()
        {
            _mockFactory.VerifyAll();
        }
    }
}