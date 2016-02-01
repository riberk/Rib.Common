namespace Rib.Common.Helpers.Encrypting.Asymmetric
{
    using System.Security.Cryptography;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class MaxBlockLengthResolverTests
    {
        [TestMethod]
        public void MaxBlockLengthTest()
        {
            var maxLength = new MaxBlockLengthResolver().MaxBlockLength(new RSACryptoServiceProvider(2048));
            Assert.AreEqual(245, maxLength);
        }

        [TestMethod]
        public void MaxBlockLengthFOAEPTest()
        {
            var maxLength = new MaxBlockLengthResolver().MaxBlockLength(new RSACryptoServiceProvider(1024), true);
            Assert.AreEqual(87, maxLength);
        }

        [TestMethod]
        public void EncryptedBlockLenghtTest()
        {
            var maxLength1 = new MaxBlockLengthResolver().EncryptedBlockLenght(new RSACryptoServiceProvider(512));
            var maxLength2 = new MaxBlockLengthResolver().EncryptedBlockLenght(new RSACryptoServiceProvider(1024));
            var maxLength3 = new MaxBlockLengthResolver().EncryptedBlockLenght(new RSACryptoServiceProvider(2048));
            Assert.AreEqual(64, maxLength1);
            Assert.AreEqual(128, maxLength2);
            Assert.AreEqual(256, maxLength3);
        }
    }
}