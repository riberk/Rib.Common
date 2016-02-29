namespace Rib.Common.Helpers.Encrypting.Symmetric
{
    using System.Security.Cryptography;
    using Rib.Common.Models.Encrypting;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(SymmetricAlgorithmFactory))]
    public interface ISymmetricAlgorithmFactory
    {
        [NotNull]
        SymmetricAlgorithm Create(SymmetricAlgorithmType algorithmType);
    }
}