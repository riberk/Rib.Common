namespace Rib.Common.Models.Encrypting
{
    using System;
    using JetBrains.Annotations;

    public class SymmetricKey : ISymmetricKey
    {
        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="T:System.Object" />.
        /// </summary>
        public SymmetricKey([NotNull] byte[] key, [NotNull] byte[] iv)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));
            if (iv == null) throw new ArgumentNullException(nameof(iv));
            Key = key;
            IV = iv;
        }

        public byte[] Key { get; }
        public byte[] IV { get; }
    }
}