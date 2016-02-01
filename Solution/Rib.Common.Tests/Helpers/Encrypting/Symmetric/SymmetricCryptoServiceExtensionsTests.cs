namespace Rib.Common.Helpers.Encrypting.Symmetric
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Rib.Common.Models.Encrypting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class SymmetricCryptoServiceExtensionsTests
    {
        private readonly SymmetricKey _emptyKey = new SymmetricKey(new byte[0], new byte[0]);
        private MockRepository _mockFactory;
        private Mock<ISymmetricCryptoService> _service;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _service = _mockFactory.Create<ISymmetricCryptoService>();
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task EncryptStringAsyncNullArgument1()
            => await SymmetricCryptoServiceExtensions.EncryptAsync(null, "123", _emptyKey, SymmetricAlgorithmType.Rijndael);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task EncryptStringAsyncNullArgument2()
            => await _service.Object.EncryptAsync((string) null, _emptyKey, SymmetricAlgorithmType.Rijndael);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task EncryptStringAsyncNullArgument3() => await _service.Object.EncryptAsync("123", null, SymmetricAlgorithmType.Rijndael);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task EncryptByteAsyncNullArgument1()
            => await SymmetricCryptoServiceExtensions.EncryptAsync(null, new byte[0], _emptyKey, SymmetricAlgorithmType.Rijndael);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task EncryptByteAsyncNullArgument2() => await _service.Object.EncryptAsync(null, _emptyKey, SymmetricAlgorithmType.Rijndael);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task EncryptByteAsyncNullArgument3() => await _service.Object.EncryptAsync(new byte[0], null, SymmetricAlgorithmType.Rijndael);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task DecryptByteAsyncNullArgument1()
            => await SymmetricCryptoServiceExtensions.DecryptAsync(null, new byte[0], _emptyKey, SymmetricAlgorithmType.Rijndael);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task DecryptByteAsyncNullArgument2() => await _service.Object.DecryptAsync(null, _emptyKey, SymmetricAlgorithmType.Rijndael);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task DecryptByteAsyncNullArgument3() => await _service.Object.DecryptAsync(new byte[0], null, SymmetricAlgorithmType.Rijndael);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task DecryptToStringAsyncNullArgument1()
            => await SymmetricCryptoServiceExtensions.DecryptToStringAsync(null, new byte[0], _emptyKey, SymmetricAlgorithmType.Rijndael);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task DecryptToStringAsyncNullArgument2()
            => await _service.Object.DecryptToStringAsync(null, _emptyKey, SymmetricAlgorithmType.Rijndael);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task DecryptToStringAsyncNullArgument3()
            => await _service.Object.DecryptToStringAsync(new byte[0], null, SymmetricAlgorithmType.Rijndael);

        [TestMethod]
        public async Task EncryptStringAsyncTest()
        {
            const string expected = "1";
            const string s = "123";
            _service.Setup(x => x.EncryptAsync(It.IsAny<Stream>(), It.IsAny<Stream>(), _emptyKey, SymmetricAlgorithmType.Rijndael))
                .Returns((Stream i, Stream o, ISymmetricKey sk, SymmetricAlgorithmType type) =>
                {
                    var msi = (MemoryStream) i;
                    var mso = (MemoryStream) o;
                    Assert.AreEqual(0, o.Length);
                    using (var sw = new StreamWriter(mso))
                    {
                        sw.Write(expected);
                    }
                    CollectionAssert.AreEqual(Encoding.UTF8.GetBytes(s), msi.ToArray());
                    return Task.CompletedTask;
                }).Verifiable();
            var res = await _service.Object.EncryptAsync(s, _emptyKey, SymmetricAlgorithmType.Rijndael);
            Assert.AreEqual(expected, Encoding.UTF8.GetString(res));
        }

        [TestMethod]
        public async Task EncryptBytesAsyncTest()
        {
            var expected = new byte[] {1, 2, 3};
            var s = new byte[] {3, 2, 1};
            _service.Setup(x => x.EncryptAsync(It.IsAny<Stream>(), It.IsAny<Stream>(), _emptyKey, SymmetricAlgorithmType.Rijndael))
                .Returns((Stream i, Stream o, ISymmetricKey sk, SymmetricAlgorithmType type) =>
                {
                    var msi = (MemoryStream) i;
                    var mso = (MemoryStream) o;
                    Assert.AreEqual(0, o.Length);
                    mso.Write(expected, 0, expected.Length);
                    CollectionAssert.AreEqual(s, msi.ToArray());
                    return Task.CompletedTask;
                }).Verifiable();
            var res = await _service.Object.EncryptAsync(s, _emptyKey, SymmetricAlgorithmType.Rijndael);
            CollectionAssert.AreEqual(expected, res);
        }

        [TestMethod]
        public async Task DecryptTest()
        {
            var source = new byte[] {1, 2, 3};
            var exp = new byte[] {3, 2, 1};
            _service.Setup(x => x.DecryptAsync(It.IsAny<Stream>(), It.IsAny<Stream>(), _emptyKey, SymmetricAlgorithmType.Rijndael))
                .Returns((Stream i, Stream o, ISymmetricKey sk, SymmetricAlgorithmType type) =>
                {
                    var msi = (MemoryStream) i;
                    var mso = (MemoryStream) o;
                    Assert.AreEqual(0, o.Length);
                    mso.Write(exp, 0, exp.Length);
                    CollectionAssert.AreEqual(source, msi.ToArray());
                    return Task.CompletedTask;
                }).Verifiable();
            var actual = await _service.Object.DecryptAsync(source, _emptyKey, SymmetricAlgorithmType.Rijndael);
            CollectionAssert.AreEqual(exp, actual);
        }

        [TestMethod]
        public async Task DecryptToStringAsyncTest()
        {
            const string expected = "1";
            var source = new byte[] {1, 2, 3, 5, 6};
            _service.Setup(x => x.DecryptAsync(It.IsAny<Stream>(), It.IsAny<Stream>(), _emptyKey, SymmetricAlgorithmType.Rijndael))
                .Returns((Stream i, Stream o, ISymmetricKey sk, SymmetricAlgorithmType type) =>
                {
                    var msi = (MemoryStream) i;
                    var mso = (MemoryStream) o;
                    Assert.AreEqual(0, o.Length);
                    using (var sw = new StreamWriter(mso))
                    {
                        sw.Write(expected);
                    }
                    CollectionAssert.AreEqual(source, msi.ToArray());
                    return Task.CompletedTask;
                }).Verifiable();
            var res = await _service.Object.DecryptToStringAsync(source, _emptyKey, SymmetricAlgorithmType.Rijndael);
            Assert.AreEqual(expected, res);
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}