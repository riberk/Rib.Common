namespace Rib.Common.Helpers.Encrypting.Asymmetric
{
    using System.Security.Cryptography;
    using Rib.Common.Models.Metadata;

    /// <summary>
    ///     �������� ���������� Rsa
    /// </summary>
    [BindFrom(typeof(IRsaCryptoServiceProviderResolver), typeof(IRsaKeyCreator))]
    internal class RsaCryptoServiceProviderResolver : IRsaCryptoServiceProviderResolver, IRsaKeyCreator
    {
        /// <summary>
        ///     �������� ��������� �� �����.
        ///     ��������� ������ ������������ � ���������� � ����� ���������
        /// </summary>
        /// <param name="containerName">��� ����������</param>
        /// <returns>��������� RSA</returns>
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