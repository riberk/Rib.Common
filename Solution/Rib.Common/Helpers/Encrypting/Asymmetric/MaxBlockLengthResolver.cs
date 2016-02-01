namespace Rib.Common.Helpers.Encrypting.Asymmetric
{
    using System.Security.Cryptography;

    public class MaxBlockLengthResolver : IMaxBlockLengthResolver
    {
        public int MaxBlockLength(AsymmetricAlgorithm provider, bool fOAEP = false)
        {
            return (provider.KeySize - 384)/8 + (fOAEP ? 7 : 37);
        }

        public int EncryptedBlockLenght(AsymmetricAlgorithm provider)
        {
            return provider.KeySize/8;
        }
    }
}