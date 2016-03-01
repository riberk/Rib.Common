namespace Rib.Common.Helpers.Encrypting.Asymmetric
{
    using System.Security.Cryptography;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(MaxBlockLengthResolver))]
    public interface IMaxBlockLengthResolver
    {
        /// <summary>
        ///     Максимальный размер блока, который можно зашифровать
        /// </summary>
        /// <param name="provider">Ассиметричный алгоритм</param>
        /// <param name="fOAEP">true to use OAEP padding (PKCS #1 v2), false to use PKCS #1 type 2 padding</param>
        /// <returns>Размер блока</returns>
        int MaxBlockLength([NotNull] AsymmetricAlgorithm provider, bool fOAEP = false);

        int EncryptedBlockLenght([NotNull] AsymmetricAlgorithm provider);
    }
}