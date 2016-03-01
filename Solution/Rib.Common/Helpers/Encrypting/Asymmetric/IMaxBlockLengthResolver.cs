namespace Rib.Common.Helpers.Encrypting.Asymmetric
{
    using System.Security.Cryptography;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(MaxBlockLengthResolver))]
    public interface IMaxBlockLengthResolver
    {
        /// <summary>
        ///     ������������ ������ �����, ������� ����� �����������
        /// </summary>
        /// <param name="provider">������������� ��������</param>
        /// <param name="fOAEP">true to use OAEP padding (PKCS #1 v2), false to use PKCS #1 type 2 padding</param>
        /// <returns>������ �����</returns>
        int MaxBlockLength([NotNull] AsymmetricAlgorithm provider, bool fOAEP = false);

        int EncryptedBlockLenght([NotNull] AsymmetricAlgorithm provider);
    }
}