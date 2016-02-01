namespace Rib.Common.Helpers.Encrypting.Symmetric
{
    using System;
    using System.IO;
    using System.Text;
    using System.Threading.Tasks;
    using Rib.Common.Models.Encrypting;
    using JetBrains.Annotations;

    public static class SymmetricCryptoServiceExtensions
    {
        public static async Task<byte[]> EncryptAsync([NotNull] this ISymmetricCryptoService service, string s, [NotNull] ISymmetricKey symmetricKey,
            SymmetricAlgorithmType type, Encoding encoding = null)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (symmetricKey == null) throw new ArgumentNullException(nameof(symmetricKey));
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            using (var source = new MemoryStream(encoding.GetBytes(s)))
            using (var encrypted = new MemoryStream())
            {
                await service.EncryptAsync(source, encrypted, symmetricKey, type);
                return encrypted.ToArray();
            }
        }

        public static async Task<byte[]> EncryptAsync([NotNull] this ISymmetricCryptoService service, byte[] s, [NotNull] ISymmetricKey symmetricKey,
            SymmetricAlgorithmType type)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (symmetricKey == null) throw new ArgumentNullException(nameof(symmetricKey));

            using (var source = new MemoryStream(s))
            using (var encrypted = new MemoryStream())
            {
                await service.EncryptAsync(source, encrypted, symmetricKey, type);
                return encrypted.ToArray();
            }
        }

        public static async Task<byte[]> DecryptAsync([NotNull] this ISymmetricCryptoService service, byte[] s, [NotNull] ISymmetricKey symmetricKey,
            SymmetricAlgorithmType type)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (symmetricKey == null) throw new ArgumentNullException(nameof(symmetricKey));
            using (var source = new MemoryStream(s))
            using (var decrypted = new MemoryStream())
            {
                await service.DecryptAsync(source, decrypted, symmetricKey, type);
                return decrypted.ToArray();
            }
        }

        public static async Task<string> DecryptToStringAsync([NotNull] this ISymmetricCryptoService service, byte[] s,
            [NotNull] ISymmetricKey symmetricKey,
            SymmetricAlgorithmType type, Encoding encoding = null)
        {
            if (service == null) throw new ArgumentNullException(nameof(service));
            if (symmetricKey == null) throw new ArgumentNullException(nameof(symmetricKey));
            if (encoding == null)
            {
                encoding = Encoding.UTF8;
            }

            using (var source = new MemoryStream(s))
            using (var decrypted = new MemoryStream())
            {
                await service.DecryptAsync(source, decrypted, symmetricKey, type);
                return encoding.GetString(decrypted.ToArray());
            }
        }
    }
}