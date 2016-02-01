namespace Rib.Common.Helpers.Encrypting.Symmetric
{
    using System;
    using System.Security.Cryptography;
    using Rib.Common.Models.Encrypting;

    internal class SymmetricAlgorithmFactory : ISymmetricAlgorithmFactory
    {
        public SymmetricAlgorithm Create(SymmetricAlgorithmType algorithmType)
        {
            switch (algorithmType)
            {
                case SymmetricAlgorithmType.Rijndael:
                    return new RijndaelManaged();
                case SymmetricAlgorithmType.AES:
                    return new AesManaged();
                default:
                    throw new ArgumentOutOfRangeException(nameof(algorithmType), algorithmType, null);
            }
        }
    }
}