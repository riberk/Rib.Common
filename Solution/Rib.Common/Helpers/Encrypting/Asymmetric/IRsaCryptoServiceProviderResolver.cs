namespace Rib.Common.Helpers.Encrypting.Asymmetric
{
    using System.Security.Cryptography;
    using JetBrains.Annotations;

    /// <summary>
    ///     Получает контейнеры Rsa
    /// </summary>
    public interface IRsaCryptoServiceProviderResolver
    {
        /// <summary>
        ///     Получает контейнер по имени.
        ///     Контейнер должен существовать и находиться в общем хранилище
        /// </summary>
        /// <param name="containerName">Имя контейнера</param>
        /// <returns>Провайдер RSA</returns>
        [NotNull]
        RSACryptoServiceProvider Get(string containerName);
    }
}