namespace Rib.Common.Helpers.Encrypting.Symmetric
{
    using System;
    using System.Security.Cryptography;
    using Rib.Common.Models.Encrypting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;

    [TestClass]
    public class SymmetricKeyFactoryTests
    {
        private Mock<ISymmetricAlgorithmFactory> _algFactory;
        private MockRepository _mockFactory;
        private Mock<ISymmetricAlgorithmTypeReader> _reader;

        [TestInitialize]
        public void Init()
        {
            _mockFactory = new MockRepository(MockBehavior.Strict);
            _algFactory = _mockFactory.Create<ISymmetricAlgorithmFactory>();
            _reader = _mockFactory.Create<ISymmetricAlgorithmTypeReader>();
        }

        [TestMethod]
        public void Constructor() => new SymmetricKeyFactory(_reader.Object, _algFactory.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorArgNull1() => new SymmetricKeyFactory(null, _algFactory.Object);

        [TestMethod]
        [ExpectedException(typeof (ArgumentNullException))]
        public void ConstructorArgNull2() => new SymmetricKeyFactory(_reader.Object, null);


        [TestMethod]
        public void CreateTest()
        {
            var rijndaelManaged = new RijndaelManaged();
            _algFactory.Setup(x => x.Create(SymmetricAlgorithmType.Rijndael)).Returns(rijndaelManaged).Verifiable();
            _reader.Setup(x => x.Read()).Returns(SymmetricAlgorithmType.Rijndael).Verifiable();
            var symmetricKeyFactory = new SymmetricKeyFactory(_reader.Object, _algFactory.Object);
            var key = symmetricKeyFactory.Create();
            var key2 = symmetricKeyFactory.Create();
            CollectionAssert.AreNotEqual(key.IV, key2.IV);
            CollectionAssert.AreNotEqual(key.Key, key2.Key);
        }

        [TestCleanup]
        public void Clean()
        {
            _mockFactory.VerifyAll();
        }
    }
}