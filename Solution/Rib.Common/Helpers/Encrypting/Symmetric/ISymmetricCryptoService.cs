namespace Rib.Common.Helpers.Encrypting.Symmetric
{
    using System.IO;
    using System.Threading.Tasks;
    using Rib.Common.Models.Encrypting;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(SymmetricCryptoService))]
    public interface ISymmetricCryptoService
    {
        Task EncryptAsync([NotNull] Stream @in, [NotNull] Stream @out, [NotNull] ISymmetricKey symmetricKey, SymmetricAlgorithmType type);
        Task DecryptAsync([NotNull] Stream @in, [NotNull] Stream @out, [NotNull] ISymmetricKey symmetricKey, SymmetricAlgorithmType type);
    }
}