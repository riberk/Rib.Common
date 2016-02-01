namespace Rib.Common.Helpers.Encrypting.Asymmetric
{
    using System;
    using System.Security.Cryptography;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RsaCryptoServiceProviderResolverTests
    {
        private RsaCryptoServiceProviderResolver _resolver;

        [TestInitialize]
        public void Init()
        {
            _resolver = new RsaCryptoServiceProviderResolver();
        }

        private static CspParameters GetCsp(string containerName)
        {
            var csp = new CspParameters
            {
                KeyContainerName = containerName,
                Flags =
                    CspProviderFlags.UseMachineKeyStore | (DoesKeyExists(containerName) ? CspProviderFlags.UseExistingKey : CspProviderFlags.NoFlags)
            };
            using (var provider = new RSACryptoServiceProvider(csp))
            {
                provider.PersistKeyInCsp = false;
                provider.Clear();
            }
            return new CspParameters
            {
                KeyContainerName = containerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };
        }

        private static bool DoesKeyExists(string name)
        {
            var cspParams = new CspParameters
            {
                Flags = CspProviderFlags.UseExistingKey | CspProviderFlags.UseMachineKeyStore,
                KeyContainerName = name
            };

            try
            {
                var provider = new RSACryptoServiceProvider(cspParams);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        [TestMethod]
        public void GetTest()
        {
            const string keyContainerName = "RsaCryptoServiceProviderResolverTestsGetTest11";
            var csp = GetCsp(keyContainerName);
            using (var provider = new RSACryptoServiceProvider(csp))
            {
                provider.PersistKeyInCsp = false;
                provider.Clear();
            }
            using (var exp = new RSACryptoServiceProvider(csp))
            using (var actual = _resolver.Get(keyContainerName))
            {
                Assert.AreEqual(exp.ToXmlString(true), actual.ToXmlString(true));
            }
        }

        [TestMethod]
        [ExpectedException(typeof (CryptographicException))]
        public void GetWithExceptionBecauseNotFoundOnMachineTest()
        {
            const string keyContainerName = "RsaCryptoServiceProviderResolverTestsGetWithExceptionBecauseNotFoundOnMachineTest";
            var csp = new CspParameters
            {
                KeyContainerName = keyContainerName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            using (var provider = new RSACryptoServiceProvider(csp))
            {
                provider.PersistKeyInCsp = false;
                provider.Clear();
            }
            _resolver.Get(keyContainerName);
        }
    }
}