﻿namespace Rib.Common.Helpers.Encrypting.Asymmetric
{
    using System;
    using System.Security.Cryptography;
    using JetBrains.Annotations;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class RsaCryptoServiceProviderResolverTests
    {
        [NotNull] private RsaCryptoServiceProviderResolver _resolver;

        [TestInitialize]
        public void Init()
        {
            if (!AsymmetricCryptoServiceTests.IsAdministrator())
            {
                Assert.Fail("All tests for crypto conteiners need administrative privileges");
            }
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

        private static void Clear(string keyName)
        {
            var csp = new CspParameters
            {
                KeyContainerName = keyName,
                Flags = CspProviderFlags.UseMachineKeyStore
            };
            using (var provider = new RSACryptoServiceProvider(csp))
            {
                provider.PersistKeyInCsp = false;
                provider.Clear();
            }
        }

        [TestMethod]
        public void GetTest()
        {
            const string keyContainerName = "RsaCryptoServiceProviderResolverTestsGetTest11";
            Clear(keyContainerName);
            var csp = GetCsp(keyContainerName);
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
            Clear(keyContainerName);
            _resolver.Get(keyContainerName);
        }

        [TestMethod]
        public void TestCreateIfNotExists()
        {
            const string keyContainerName = "RsaCryptoServiceProviderResolverTestCreateIfNotExists";
            Assert.IsFalse(DoesKeyExists(keyContainerName));
            _resolver.CreateIfNotExists(keyContainerName);
            Assert.IsTrue(DoesKeyExists(keyContainerName));
            Clear(keyContainerName);
            Assert.IsFalse(DoesKeyExists(keyContainerName));
        }
    }
}