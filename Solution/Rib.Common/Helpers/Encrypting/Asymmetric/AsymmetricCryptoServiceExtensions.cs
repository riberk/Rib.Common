namespace Rib.Common.Helpers.Encrypting.Asymmetric
{
    using System;
    using System.Text;
    using JetBrains.Annotations;

    /// <summary>
    ///     ������ ���������� ��� <see cref="IAsymmetricCryptoService" />
    /// </summary>
    public static class AsymmetricCryptoServiceExtensions
    {
        /// <summary>
        ///     ����������� ������
        /// </summary>
        /// <param name="cryptoService">��������� ������ <see cref="IAsymmetricCryptoService" /></param>
        /// <param name="source">�������� ������</param>
        /// <param name="containerName">��� ���������� ������</param>
        /// <param name="encoding">������������ ���������, �� ��������� <see cref="Encoding.UTF8" /></param>
        /// <returns>����������� ������������������ ����</returns>
        [NotNull]
        public static byte[] Encrypt([NotNull] this IAsymmetricCryptoService cryptoService, [NotNull] string source, [NotNull] string containerName,
            Encoding encoding = null)
        {
            if (cryptoService == null) throw new ArgumentNullException(nameof(cryptoService));
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (containerName == null) throw new ArgumentNullException(nameof(containerName));

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }
            return cryptoService.Encrypt(encoding.GetBytes(source), containerName);
        }

        /// <summary>
        ///     ����������� <see cref="Guid" />
        /// </summary>
        /// <param name="cryptoService">��������� ������ <see cref="IAsymmetricCryptoService" /></param>
        /// <param name="source">�������� <see cref="Guid" /></param>
        /// <param name="containerName">��� ���������� ������</param>
        /// <returns>����������� ������������������ ����</returns>
        public static byte[] Encrypt([NotNull] this IAsymmetricCryptoService cryptoService, Guid source, [NotNull] string containerName)
        {
            if (cryptoService == null) throw new ArgumentNullException(nameof(cryptoService));
            if (containerName == null) throw new ArgumentNullException(nameof(containerName));
            return cryptoService.Encrypt(source.ToByteArray(), containerName);
        }

        /// <summary>
        ///     ������������ ������
        /// </summary>
        /// <param name="cryptoService">��������� ������ <see cref="IAsymmetricCryptoService" /></param>
        /// <param name="source">����������� �����</param>
        /// <param name="containerName">��� ���������� ������</param>
        /// <param name="encoding">������������ ���������, �� ��������� <see cref="Encoding.UTF8" /></param>
        /// <returns>�������������� ������</returns>
        [NotNull]
        public static string DecryptToString([NotNull] this IAsymmetricCryptoService cryptoService, [NotNull] byte[] source,
            [NotNull] string containerName,
            Encoding encoding = null)
        {
            if (cryptoService == null) throw new ArgumentNullException(nameof(cryptoService));
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (containerName == null) throw new ArgumentNullException(nameof(containerName));

            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            var res = cryptoService.Decrypt(source, containerName);
            return encoding.GetString(res);
        }

        /// <summary>
        ///     ������������ <see cref="Guid" />
        /// </summary>
        /// <param name="cryptoService">��������� ������ <see cref="IAsymmetricCryptoService" /></param>
        /// <param name="source">����������� �����</param>
        /// <param name="containerName">��� ���������� ������</param>
        /// <returns>�������������� <see cref="Guid" /></returns>
        public static Guid DecryptToGuid([NotNull] this IAsymmetricCryptoService cryptoService, [NotNull] byte[] source,
            [NotNull] string containerName)
        {
            if (cryptoService == null) throw new ArgumentNullException(nameof(cryptoService));
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (containerName == null) throw new ArgumentNullException(nameof(containerName));
            var res = cryptoService.Decrypt(source, containerName);
            return new Guid(res);
        }
    }
}