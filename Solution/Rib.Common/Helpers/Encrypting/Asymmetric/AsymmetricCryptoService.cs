namespace Rib.Common.Helpers.Encrypting.Asymmetric
{
    using System;
    using System.Linq;
    using JetBrains.Annotations;

    /// <summary>
    ///     ��������/����������
    /// </summary>
    internal class AsymmetricCryptoService : IAsymmetricCryptoService
    {
        [NotNull] private readonly IByteArraySplitter _byteArraySplitter;
        [NotNull] private readonly IMaxBlockLengthResolver _maxBlockLengthResolver;
        [NotNull] private readonly IRsaCryptoServiceProviderResolver _rsaCryptoServiceProviderResolver;

        public AsymmetricCryptoService([NotNull] IRsaCryptoServiceProviderResolver rsaCryptoServiceProviderResolver,
            [NotNull] IByteArraySplitter byteArraySplitter, [NotNull] IMaxBlockLengthResolver maxBlockLengthResolver)
        {
            if (rsaCryptoServiceProviderResolver == null) throw new ArgumentNullException(nameof(rsaCryptoServiceProviderResolver));
            if (byteArraySplitter == null) throw new ArgumentNullException(nameof(byteArraySplitter));
            if (maxBlockLengthResolver == null) throw new ArgumentNullException(nameof(maxBlockLengthResolver));
            _rsaCryptoServiceProviderResolver = rsaCryptoServiceProviderResolver;
            _byteArraySplitter = byteArraySplitter;
            _maxBlockLengthResolver = maxBlockLengthResolver;
        }

        /// <summary>
        ///     ����������� ��������
        /// </summary>
        /// <param name="source">�������� ������������������ ����</param>
        /// <param name="containerName">��� ����������</param>
        /// <returns>������������� ������������������ ����</returns>
        public byte[] Encrypt(byte[] source, string containerName)
        {
            using (var provider = _rsaCryptoServiceProviderResolver.Get(containerName))
            {
                var splittedBytes = _byteArraySplitter.Split(source, _maxBlockLengthResolver.MaxBlockLength(provider));
                return splittedBytes.SelectMany(x => provider.Encrypt(x, false)).ToArray();
            }
        }

        /// <summary>
        ///     ������������ ��������
        /// </summary>
        /// <param name="source">�������� ������������������ ����</param>
        /// <param name="containerName">��� ����������</param>
        /// <returns>�������������� ������������������ ����</returns>
        public byte[] Decrypt(byte[] source, string containerName)
        {
            using (var provider = _rsaCryptoServiceProviderResolver.Get(containerName))
            {
                return
                    _byteArraySplitter.Split(source, _maxBlockLengthResolver.EncryptedBlockLenght(provider))
                        .SelectMany(x => provider.Decrypt(x, false))
                        .ToArray();
            }
        }
    }
}