namespace Rib.Common.Helpers.Cache
{
    using System;
    using JetBrains.Annotations;

    public class CacheUpdatedEventArgs : EventArgs
    {
        /// <summary>
        /// »нициализирует новый экземпл€р класса <see cref="T:System.EventArgs"/>.
        /// </summary>
        public CacheUpdatedEventArgs([NotNull] string fullKey)
        {
            if (string.IsNullOrWhiteSpace(fullKey)) throw new ArgumentNullException(nameof(fullKey));
            FullKey = fullKey;
        }

        [NotNull]
        public string FullKey { get; }
    }
}