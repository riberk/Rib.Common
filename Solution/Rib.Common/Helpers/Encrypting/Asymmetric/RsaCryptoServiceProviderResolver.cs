namespace Rib.Common.Helpers.Encrypting.Asymmetric
{
    using System.Security.Cryptography;
    using Rib.Common.Models.Metadata;

    /// <summary>
    ///     Получает контейнеры Rsa
    /// </summary>
    [BindFrom(typeof(IRsaCryptoServiceProviderResolver), typeof(IRsaKeyCreator))]
    internal class RsaCryptoServiceProviderResolver : IRsaCryptoServiceProviderResolver, IRsaKeyCreator
    {
        /// <summary>
        ///     Получает контейнер по имени.
        ///     Контейнер должен существовать и находиться в общем хранилище
        /// </summary>
        /// <param name="containerName">Имя контейнера</param>
        /// <returns>Провайдер RSA</returns>
        public RSACryptoServiceProvider Get(string containerName)
        {
            var cspParameters = new CspParameters
            {
                KeyContainerName = containerName,
                Flags = CspProviderFlags.UseMachineKeyStore | CspProviderFlags.UseExistingKey
            };
            return new RSACryptoServiceProvider(cspParameters);
        }

        public void CreateIfNotExists(string containerName)
        {
            var cspParameters = new CspParameters
            {
                KeyContainerName = containerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            using (new RSACryptoServiceProvider(cspParameters))
            {
            }
        }
    }
}