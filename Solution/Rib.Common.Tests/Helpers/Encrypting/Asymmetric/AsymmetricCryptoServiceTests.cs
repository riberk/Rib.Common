namespace Rib.Common.Helpers.Encrypting.Asymmetric
{
    using System;
    using System.Linq;
    using System.Security.Cryptography;
    using System.Text;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class AsymmetricCryptoServiceTests
    {
        private Mock<IByteArraySplitter> _byteSplitter;
        private Mock<IMaxBlockLengthResolver> _maxBlockLengthResolver;
        private MockRepository _mockFactory;
        [NotNull] private Mock<IRsaCryptoServiceProviderResolver> _resolverMock;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _resolverMock = _mockFactory.Create<IRsaCryptoServiceProviderResolver>();
            _byteSplitter = _mockFactory.Create<IByteArraySplitter>();
            _maxBlockLengthResolver = _mockFactory.Create<IMaxBlockLengthResolver>();
        }

        [TestMethod]
        public void Constructor() => new AsymmetricCryptoService(_resolverMock.Object, _byteSplitter.Object, _maxBlockLengthResolver.Object);

        private static CspParameters GetCsp(string containerName)
        {
            var csp = new CspParameters
            {
                KeyContainerName = containerName,
                Flags =
                    CspProviderFlags.UseMachineKeyStore | (DoesKeyExists(containerName) ? CspProviderFlags.UseExistingKey : CspProviderFlags.NoFlags)
            };
            using (var provider = new RSACryptoServiceProvider(csp))
            {
                provider.PersistKeyInCsp = false;
                provider.Clear();
            }
            return new CspParameters
            {
                KeyContainerName = containerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };
        }

        private static bool DoesKeyExists(string name)
        {
            var cspParams = new CspParameters
            {
                Flags = CspProviderFlags.UseExistingKey | CspProviderFlags.UseMachineKeyStore,
                KeyContainerName = name
            };

            try
            {
                new RSACryptoServiceProvider(cspParams);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        [TestMethod]
        public void EncryptTest()
        {
            var containerName = "EncryptTest";
            var csp = GetCsp(containerName);
            var cryptoService = new AsymmetricCryptoService(_resolverMock.Object, _byteSplitter.Object, _maxBlockLengthResolver.Object);
            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider(csp))
            using (var currentRsaProvider = new RSACryptoServiceProvider(csp))
            {
                _resolverMock.Setup(x => x.Get(containerName)).Returns(rsaCryptoServiceProvider).Verifiable();
                var maxLength = 256;
                _maxBlockLengthResolver.Setup(x => x.MaxBlockLength(rsaCryptoServiceProvider, false)).Returns(maxLength).Verifiable();
                var expected = Enumerable.Range(0, 10).Select(x => x.ToString()).Aggregate("", (s, s1) => $"{s}{s1}");
                var expBytes = Encoding.UTF8.GetBytes(expected);
                _byteSplitter.Setup(x => x.Split(expBytes, maxLength)).Returns(new[] {expBytes}).Verifiable();
                var res = cryptoService.Encrypt(expBytes, containerName);
                CollectionAssert.AreNotEqual(expBytes, res);

                var actualBytes = Split(res, currentRsaProvider.KeySize/8).SelectMany(x => currentRsaProvider.Decrypt(x, false)).ToArray();
                var actual = Encoding.UTF8.GetString(actualBytes);
                CollectionAssert.AreEqual(expBytes, actualBytes);
                Assert.AreEqual(expected, actual);
            }
        }

        private static byte[][] Split(byte[] ar, int ml)
        {
            var blocksCount = (int) Math.Ceiling((double) ar.Length/ml);
            var b = new byte[blocksCount][];
            for (var i = 0; i < blocksCount; i++)
            {
                var cur = ar.Skip(i*ml).Take(ml).ToArray();
                b[i] = cur;
            }
            return b;
        }

        [TestMethod]
        public void DifferentTest()
        {
            var containerName = "DifferentTest";
            var csp = GetCsp(containerName);
            var cryptoService = new AsymmetricCryptoService(_resolverMock.Object, _byteSplitter.Object, _maxBlockLengthResolver.Object);
            using (var currentRsaProvider = new RSACryptoServiceProvider(csp))
            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider(csp))
            {
                Assert.AreEqual(currentRsaProvider.ToXmlString(true), rsaCryptoServiceProvider.ToXmlString(true));
                _resolverMock.Setup(x => x.Get(containerName)).Returns(rsaCryptoServiceProvider).Verifiable();
                var expected = "123";
                var expBytes = Encoding.UTF8.GetBytes(expected);
                var maxLength = 100;
                _maxBlockLengthResolver.Setup(x => x.MaxBlockLength(rsaCryptoServiceProvider, false)).Returns(maxLength).Verifiable();
                _maxBlockLengthResolver.Setup(x => x.MaxBlockLength(currentRsaProvider, false)).Returns(maxLength).Verifiable();
                _byteSplitter.Setup(x => x.Split(expBytes, maxLength)).Returns(new[] {expBytes}).Verifiable();
                var res1 = cryptoService.Encrypt(expBytes, containerName);
                _resolverMock.Setup(x => x.Get(containerName)).Returns(currentRsaProvider).Verifiable();
                var res2 = cryptoService.Encrypt(expBytes, containerName);
                CollectionAssert.AreNotEqual(res1, res2);
            }
        }

        [TestMethod]
        public void DecryptTest()
        {
            var containerName = "DecryptTest";
            var csp = GetCsp(containerName);
            var cryptoService = new AsymmetricCryptoService(_resolverMock.Object, _byteSplitter.Object, _maxBlockLengthResolver.Object);
            using (var rsaCryptoServiceProvider = new RSACryptoServiceProvider(csp))
            using (var currentRsaProvider = new RSACryptoServiceProvider(csp))
            {
                _resolverMock.Setup(x => x.Get(containerName)).Returns(rsaCryptoServiceProvider).Verifiable();
                var bytes = Encoding.UTF8.GetBytes("123456");
                var exp = currentRsaProvider.Encrypt(bytes, false);
                Assert.AreNotEqual(bytes, exp);
                var maxBlock = rsaCryptoServiceProvider.KeySize/8;
                _maxBlockLengthResolver.Setup(x => x.EncryptedBlockLenght(rsaCryptoServiceProvider)).Returns(maxBlock).Verifiable();
                _byteSplitter.Setup(x => x.Split(exp, maxBlock)).Returns((byte[] a, int ml) => Split(a, ml)).Verifiable();
                CollectionAssert.AreEqual(bytes, cryptoService.Decrypt(exp, containerName));
            }
        }

        [TestMethod]
        public void EncryptDecryptLongTest()
        {
            const string containerName = "EncryptDecryptLongTest";
            var csp = GetCsp(containerName);
            var cryptoService = new AsymmetricCryptoService(_resolverMock.Object, _byteSplitter.Object, _maxBlockLengthResolver.Object);
            using (var encryptRsaCryptoServiceProvider = new RSACryptoServiceProvider(csp))
            using (var decryptRsaCryptoServiceProvider = new RSACryptoServiceProvider(csp))
            {
                var expected = Enumerable.Range(0, 5000).Select(x => x.ToString()).Aggregate("", (s, s1) => $"{s}{s1}");
                var expBytes = Encoding.UTF8.GetBytes(expected);

                _resolverMock.Setup(x => x.Get(containerName)).Returns(encryptRsaCryptoServiceProvider).Verifiable();
                var maxLengt = 53;
                _maxBlockLengthResolver.Setup(x => x.MaxBlockLength(encryptRsaCryptoServiceProvider, false)).Returns(maxLengt).Verifiable();
                _byteSplitter.Setup(x => x.Split(expBytes, maxLengt)).Returns((byte[] a, int ml) => Split(a, ml)).Verifiable();
                var res = cryptoService.Encrypt(expBytes, containerName);
                CollectionAssert.AreNotEqual(expBytes, res);

                _resolverMock.Setup(x => x.Get(containerName)).Returns(decryptRsaCryptoServiceProvider).Verifiable();
                var encBlock = decryptRsaCryptoServiceProvider.KeySize/8;
                _maxBlockLengthResolver.Setup(x => x.EncryptedBlockLenght(decryptRsaCryptoServiceProvider)).Returns(encBlock).Verifiable();
                _byteSplitter.Setup(x => x.Split(res, encBlock)).Returns((byte[] a, int ml) => Split(a, encBlock)).Verifiable();
                var actual = cryptoService.Decrypt(res, containerName);
                CollectionAssert.AreEqual(expBytes, actual);
            }
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}