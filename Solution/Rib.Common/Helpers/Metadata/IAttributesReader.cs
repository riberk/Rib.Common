namespace Rib.Common.Helpers.Metadata
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using JetBrains.Annotations;

    public interface IAttributesReader
    {
        T Read<T>([NotNull] MemberInfo t) where T : Attribute;

        [NotNull, ItemNotNull]
        IReadOnlyCollection<T> ReadMany<T>([NotNull] MemberInfo t) where T : Attribute;
    }
}