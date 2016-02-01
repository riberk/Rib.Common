namespace Rib.Common.Helpers.Encrypting.Symmetric
{
    using System;
    using System.Security.Cryptography;
    using Rib.Common.Models.Encrypting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SymmetricAlgorithmFactoryTests
    {
        [TestMethod]
        public void CreateRijndaelTest()
        {
            var res = new SymmetricAlgorithmFactory().Create(SymmetricAlgorithmType.Rijndael);
            Assert.IsTrue(res is RijndaelManaged);
        }

        [TestMethod]
        public void CreateAesTest()
        {
            var res = new SymmetricAlgorithmFactory().Create(SymmetricAlgorithmType.AES);
            Assert.IsTrue(res is AesManaged);
        }

        [TestMethod]
        [ExpectedException(typeof (ArgumentOutOfRangeException))]
        public void CreateOutOfRange()
        {
            new SymmetricAlgorithmFactory().Create((SymmetricAlgorithmType) 100);
        }
    }
}