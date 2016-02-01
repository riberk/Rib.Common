namespace Rib.Common.Helpers.Encrypting.Asymmetric
{
    using System;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AsymmetricCryptoServiceExtensionsTests
    {
        private MockRepository _mockFactory;
        private Mock<IAsymmetricCryptoService> _service;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _service = _mockFactory.Create<IAsymmetricCryptoService>();
        }

        [TestMethod]
        public void GuidEncryptTest()
        {
            var newGuid = Guid.NewGuid();
            var bytes = newGuid.ToByteArray();
            const string containerName = "123";
            var exp = new byte[] {1, 2, 3};
            _service.Setup(x => x.Encrypt(bytes, containerName)).Returns(exp).Verifiable();
            var actual = _service.Object.Encrypt(newGuid, containerName);
            CollectionAssert.AreEqual(exp, actual);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void GuidEncryptNullArg1Test() => AsymmetricCryptoServiceExtensions.Encrypt(null, Guid.NewGuid(), "123");

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void GuidEncryptNullArg2Test() => _service.Object.Encrypt(Guid.NewGuid(), null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void StrEncryptNullArg3Test() => AsymmetricCryptoServiceExtensions.Encrypt(null, "123", "123");

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void StrEncryptNullArg4Test() => AsymmetricCryptoServiceExtensions.Encrypt(_service.Object, null, "123");

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void StrEncryptNullArg5Test() => _service.Object.Encrypt("123", null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void GuidDecryptNullArg1Test() => AsymmetricCryptoServiceExtensions.DecryptToGuid(null, new byte[] {1, 1}, "123");

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void GuidDecryptNullArg2Test() => _service.Object.DecryptToGuid(null, "123");

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void GuidDecryptNullArg3Test() => _service.Object.DecryptToGuid(new byte[] {1, 1}, null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void StrDecryptNullArg3Test() => AsymmetricCryptoServiceExtensions.DecryptToString(null, new byte[] {1, 1}, "123");

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void StrDecryptNullArg4Test() => _service.Object.DecryptToString(null, "123");

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void StrDecryptNullArg5Test() => _service.Object.DecryptToString(new byte[] {1, 1}, null);

        [TestMethod]
        public void StringEncryptUTF8Test()
        {
            const string str = "123";
            var bytes = Encoding.UTF8.GetBytes(str);
            const string containerName = "12311";
            var exp = new byte[] {1, 2, 3};
            _service.Setup(x => x.Encrypt(bytes, containerName)).Returns(exp).Verifiable();
            var actual = _service.Object.Encrypt(str, containerName);
            CollectionAssert.AreEqual(exp, actual);
        }

        [TestMethod]
        public void StringEncryptEncodingTest()
        {
            const string str = "123";
            var encoding = Encoding.GetEncoding("Windows-1251");
            var bytes = encoding.GetBytes(str);
            const string containerName = "12311";
            var exp = new byte[] {1, 2, 3};
            _service.Setup(x => x.Encrypt(bytes, containerName)).Returns(exp).Verifiable();
            var actual = _service.Object.Encrypt(str, containerName, encoding);
            CollectionAssert.AreEqual(exp, actual);
        }

        [TestMethod]
        public void StringDecryptUTF8Test()
        {
            var source = new byte[] {1, 2};
            const string containerName = "123";
            const string exp = "ыупаыуа";
            _service.Setup(x => x.Decrypt(source, containerName)).Returns(Encoding.UTF8.GetBytes(exp)).Verifiable();
            var actual = _service.Object.DecryptToString(source, containerName);
            Assert.AreEqual(exp, actual);
        }

        [TestMethod]
        public void StringDecryptEncodingTest()
        {
            var source = new byte[] {1, 2};
            const string containerName = "123";
            const string exp = "ыупаыуа";
            var encoding = Encoding.GetEncoding("Windows-1251");
            _service.Setup(x => x.Decrypt(source, containerName)).Returns(encoding.GetBytes(exp)).Verifiable();
            var actual = _service.Object.DecryptToString(source, containerName, encoding);
            Assert.AreEqual(exp, actual);
        }

        [TestMethod]
        public void GuidDecryptTest()
        {
            var exp = Guid.NewGuid();
            var source = new byte[] {12, 1};
            const string containerName = "123";
            _service.Setup(x => x.Decrypt(source, containerName)).Returns(exp.ToByteArray()).Verifiable();
            var actual = _service.Object.DecryptToGuid(source, containerName);
            Assert.AreEqual(exp, actual);
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}