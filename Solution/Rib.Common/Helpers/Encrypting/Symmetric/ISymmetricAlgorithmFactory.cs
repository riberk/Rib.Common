namespace Rib.Common.Helpers.Encrypting.Symmetric
{
    using System.Security.Cryptography;
    using Rib.Common.Models.Encrypting;
    using JetBrains.Annotations;

    public interface ISymmetricAlgorithmFactory
    {
        [NotNull]
        SymmetricAlgorithm Create(SymmetricAlgorithmType algorithmType);
    }
}