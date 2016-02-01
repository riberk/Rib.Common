namespace Rib.Tests.Mock
{
    using System.IO;
    using System.Threading.Tasks;
    using Rib.Common.Helpers.Encrypting.Symmetric;
    using Rib.Common.Models.Encrypting;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Moq;
    using Moq.Language.Flow;

    public static class SymmetricCryptoServiceExtensionsMocker
    {
        public static IReturnsResult<ISymmetricCryptoService> MockDecryptAsync(this Mock<ISymmetricCryptoService> service, ISymmetricKey key,
            SymmetricAlgorithmType symmetricAlgorithmType, byte[] source, byte[] exp)
        {
            return service.Setup(x => x.DecryptAsync(It.IsAny<Stream>(), It.IsAny<Stream>(), key, symmetricAlgorithmType))
                .Returns((Stream i, Stream o, ISymmetricKey sk, SymmetricAlgorithmType type) =>
                {
                    var msi = (MemoryStream) i;
                    var mso = (MemoryStream) o;
                    Assert.AreEqual(0, o.Length);
                    mso.Write(exp, 0, exp.Length);

                    CollectionAssert.AreEqual(source, msi.ToArray());
                    return Task.CompletedTask;
                });
        }
    }
}