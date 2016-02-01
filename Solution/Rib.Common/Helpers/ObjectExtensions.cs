namespace Rib.Common.Helpers
{
    using System;
    using JetBrains.Annotations;

    public static class ObjectExtensions
    {
        [NotNull]
        public static T ThrowIfNull<T>(this T value, string name) where T : class
        {
            if (value == null)
            {
                throw new NullReferenceException(name);
            }
            return value;
        }
    }
}