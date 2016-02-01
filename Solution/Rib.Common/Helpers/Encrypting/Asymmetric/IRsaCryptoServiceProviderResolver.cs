namespace Rib.Common.Helpers.Encrypting.Asymmetric
{
    using System.Security.Cryptography;
    using JetBrains.Annotations;

    /// <summary>
    ///     �������� ���������� Rsa
    /// </summary>
    public interface IRsaCryptoServiceProviderResolver
    {
        /// <summary>
        ///     �������� ��������� �� �����.
        ///     ��������� ������ ������������ � ���������� � ����� ���������
        /// </summary>
        /// <param name="containerName">��� ����������</param>
        /// <returns>��������� RSA</returns>
        [NotNull]
        RSACryptoServiceProvider Get(string containerName);
    }
}