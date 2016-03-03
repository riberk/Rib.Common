namespace Rib.Common.Helpers.Cache
{
    using System;
    using JetBrains.Annotations;

    public class CacheEventArgs : EventArgs
    {
        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="T:CacheEventArgs" />.
        /// </summary>
        public CacheEventArgs([NotNull] string fullKey)
        {
            if (fullKey == null) throw new ArgumentNullException(nameof(fullKey));
            FullKey = fullKey;
        }

        public string FullKey { get; }
    }
}