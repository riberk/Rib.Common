namespace Rib.Common.Helpers.Encrypting.Symmetric
{
    using System;
    using System.IO;
    using System.Security.Cryptography;
    using System.Threading.Tasks;
    using Rib.Common.Models.Encrypting;
    using JetBrains.Annotations;

    internal class SymmetricCryptoService : ISymmetricCryptoService
    {
        [NotNull] private readonly ISymmetricAlgorithmFactory _symmetricAlgorithmFactory;

        public SymmetricCryptoService([NotNull] ISymmetricAlgorithmFactory symmetricAlgorithmFactory)
        {
            if (symmetricAlgorithmFactory == null) throw new ArgumentNullException(nameof(symmetricAlgorithmFactory));
            _symmetricAlgorithmFactory = symmetricAlgorithmFactory;
        }

        public async Task EncryptAsync(Stream @in, Stream @out, ISymmetricKey symmetricKey, SymmetricAlgorithmType type)
        {
            if (@in == null) throw new ArgumentNullException(nameof(@in));
            if (@out == null) throw new ArgumentNullException(nameof(@out));
            if (symmetricKey == null) throw new ArgumentNullException(nameof(symmetricKey));

            using (var rm = _symmetricAlgorithmFactory.Create(type))
            using (var cs = new CryptoStream(@out, rm.CreateEncryptor(symmetricKey.Key, symmetricKey.IV), CryptoStreamMode.Write))
            {
                await @in.CopyToAsync(cs);
            }
        }

        public async Task DecryptAsync(Stream @in, Stream @out, ISymmetricKey symmetricKey, SymmetricAlgorithmType type)
        {
            if (@in == null) throw new ArgumentNullException(nameof(@in));
            if (@out == null) throw new ArgumentNullException(nameof(@out));
            if (symmetricKey == null) throw new ArgumentNullException(nameof(symmetricKey));


            using (var rm = _symmetricAlgorithmFactory.Create(type))
            using (var cs = new CryptoStream(@in, rm.CreateDecryptor(symmetricKey.Key, symmetricKey.IV), CryptoStreamMode.Read))
            {
                await cs.CopyToAsync(@out);
            }
        }
    }
}