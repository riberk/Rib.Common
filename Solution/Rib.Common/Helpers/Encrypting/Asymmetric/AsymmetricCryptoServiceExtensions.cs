namespace Rib.Common.Helpers.Encrypting.Asymmetric
{
    using System;
    using System.Text;
    using JetBrains.Annotations;

    /// <summary>
    ///     Методы расширения для <see cref="IAsymmetricCryptoService" />
    /// </summary>
    public static class AsymmetricCryptoServiceExtensions
    {
        /// <summary>
        ///     Зашифровать строку
        /// </summary>
        /// <param name="cryptoService">Экземпляр класса <see cref="IAsymmetricCryptoService" /></param>
        /// <param name="source">Исходная строка</param>
        /// <param name="containerName">Имя контейнера ключей</param>
        /// <param name="encoding">Используемая кодировка, по умолчанию <see cref="Encoding.UTF8" /></param>
        /// <returns>Шифрованная последовательность байт</returns>
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
        ///     Зашифровать <see cref="Guid" />
        /// </summary>
        /// <param name="cryptoService">Экземпляр класса <see cref="IAsymmetricCryptoService" /></param>
        /// <param name="source">Исходный <see cref="Guid" /></param>
        /// <param name="containerName">Имя контейнера ключей</param>
        /// <returns>Шифрованная последовательность байт</returns>
        public static byte[] Encrypt([NotNull] this IAsymmetricCryptoService cryptoService, Guid source, [NotNull] string containerName)
        {
            if (cryptoService == null) throw new ArgumentNullException(nameof(cryptoService));
            if (containerName == null) throw new ArgumentNullException(nameof(containerName));
            return cryptoService.Encrypt(source.ToByteArray(), containerName);
        }

        /// <summary>
        ///     Расшифровать строку
        /// </summary>
        /// <param name="cryptoService">Экземпляр класса <see cref="IAsymmetricCryptoService" /></param>
        /// <param name="source">Шифрованные байты</param>
        /// <param name="containerName">Имя контейнера ключей</param>
        /// <param name="encoding">Используемая кодировка, по умолчанию <see cref="Encoding.UTF8" /></param>
        /// <returns>Расшифрованная строка</returns>
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
        ///     Расшифровать <see cref="Guid" />
        /// </summary>
        /// <param name="cryptoService">Экземпляр класса <see cref="IAsymmetricCryptoService" /></param>
        /// <param name="source">Шифрованные байты</param>
        /// <param name="containerName">Имя контейнера ключей</param>
        /// <returns>Расшифрованный <see cref="Guid" /></returns>
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