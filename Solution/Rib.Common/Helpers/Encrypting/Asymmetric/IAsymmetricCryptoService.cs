namespace Rib.Common.Helpers.Encrypting.Asymmetric
{
    using JetBrains.Annotations;

    /// <summary>
    ///     Шифратор/дешифратор
    /// </summary>
    public interface IAsymmetricCryptoService
    {
        /// <summary>
        ///     Зашифровать значение
        /// </summary>
        /// <param name="source">Исходная последовательность байт</param>
        /// <param name="containerName">Имя контейнера</param>
        /// <returns>Зашифрованная последовательность байт</returns>
        [NotNull]
        byte[] Encrypt([NotNull] byte[] source, [NotNull] string containerName);

        /// <summary>
        ///     Расшифровать значение
        /// </summary>
        /// <param name="source">Исходная последовательность байт</param>
        /// <param name="containerName">Имя контейнера</param>
        /// <returns>Расшифрованная последовательность байт</returns>
        [NotNull]
        byte[] Decrypt([NotNull] byte[] source, [NotNull] string containerName);
    }
}