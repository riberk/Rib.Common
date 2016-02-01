namespace Rib.Common.Helpers.Encrypting.Symmetric
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using Rib.Common.Models.Encrypting;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class SymmetricCryptoServiceTests
    {
        [NotNull] private static readonly byte[] Key =
        {
            0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16
        };

        [NotNull] private static readonly byte[] IV = {0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16};

        [NotNull] private Mock<ISymmetricAlgorithmFactory> _algFactory;

        [NotNull] private MockRepository _mockFactory;

        [NotNull]
        private static SymmetricKey EmptyKey => new SymmetricKey(new byte[0], new byte[0]);

        [NotNull]
        private static SymmetricKey FullKey => new SymmetricKey(Key, IV);

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _algFactory = _mockFactory.Create<ISymmetricAlgorithmFactory>();
        }

        [TestMethod]
        public void Constructor() => new SymmetricCryptoService(_algFactory.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorNullArgument() => new SymmetricCryptoService(null);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task EncryptAsyncNullArgument1()
            => await new SymmetricCryptoService(_algFactory.Object).EncryptAsync(null, Stream.Null, EmptyKey, SymmetricAlgorithmType.Rijndael);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task EncryptAsyncNullArgument2()
            => await new SymmetricCryptoService(_algFactory.Object).EncryptAsync(Stream.Null, null, EmptyKey, SymmetricAlgorithmType.Rijndael);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task EncryptAsyncNullArgument3()
            => await new SymmetricCryptoService(_algFactory.Object).EncryptAsync(Stream.Null, Stream.Null, null, SymmetricAlgorithmType.Rijndael);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task DecryptAsyncNullArgument1()
            => await new SymmetricCryptoService(_algFactory.Object).DecryptAsync(null, Stream.Null, EmptyKey, SymmetricAlgorithmType.Rijndael);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task DecryptAsyncNullArgument2()
            => await new SymmetricCryptoService(_algFactory.Object).DecryptAsync(Stream.Null, null, EmptyKey, SymmetricAlgorithmType.Rijndael);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public async Task DecryptAsyncNullArgument3()
            => await new SymmetricCryptoService(_algFactory.Object).DecryptAsync(Stream.Null, Stream.Null, null, SymmetricAlgorithmType.Rijndael);

        [TestMethod]
        public async Task EncryptAsyncTest()
        {
            var rijndaelManaged = new RijndaelManaged();
            _algFactory.Setup(x => x.Create(SymmetricAlgorithmType.Rijndael)).Returns(rijndaelManaged).Verifiable();
            var inBytes = new byte[] {1, 2, 3, 4, 5};
            var service = new SymmetricCryptoService(_algFactory.Object);

            using (var msIn = new MemoryStream(inBytes))
            using (var msOut = new MemoryStream())
            using (var msDec = new MemoryStream())
            {
                await service.EncryptAsync(msIn, msOut, FullKey, SymmetricAlgorithmType.Rijndael);
                var outBytes = msOut.ToArray();
                CollectionAssert.AreNotEqual(inBytes, outBytes);
                using (var rm = new RijndaelManaged())
                using (var cs = new CryptoStream(new MemoryStream(outBytes), rm.CreateDecryptor(Key, IV), CryptoStreamMode.Read))
                {
                    await cs.CopyToAsync(msDec);
                }
                CollectionAssert.AreEqual(inBytes, msDec.ToArray());
            }
        }

        [TestMethod]
        public async Task DecryptAsyncTest()
        {
            var rijndaelManaged = new RijndaelManaged();
            _algFactory.Setup(x => x.Create(SymmetricAlgorithmType.Rijndael)).Returns(rijndaelManaged).Verifiable();
            var inBytes = new byte[] {1, 2, 3, 4, 5};
            var dest = new MemoryStream();
            using (var rm = new RijndaelManaged())
            using (var src = new MemoryStream(inBytes))
            using (var cs = new CryptoStream(dest, rm.CreateEncryptor(Key, IV), CryptoStreamMode.Write))
            {
                await src.CopyToAsync(cs);
            }
            var encrypted = dest.ToArray();
            Assert.AreNotEqual(0, encrypted.Length);
            CollectionAssert.AreNotEqual(inBytes, encrypted);
            var service = new SymmetricCryptoService(_algFactory.Object);
            using (var dec = new MemoryStream())
            {
                await service.DecryptAsync(new MemoryStream(encrypted), dec, FullKey, SymmetricAlgorithmType.Rijndael);
                CollectionAssert.AreEqual(inBytes, dec.ToArray());
            }
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}