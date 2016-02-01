namespace Rib.Common.Helpers.Metadata
{
    using System;
    using System.Reflection;
    using Rib.Common.Models.Exceptions;
    using JetBrains.Annotations;

    public static class AttributesReaderExtensions
    {
        [NotNull]
        public static T ReadSafe<T>([NotNull] this IAttributesReader attributesReader, [NotNull] MemberInfo t) where T : Attribute
        {
            if (attributesReader == null) throw new ArgumentNullException(nameof(attributesReader));
            if (t == null) throw new ArgumentNullException(nameof(t));
            var res = attributesReader.Read<T>(t);
            if (res == null)
            {
                var type = typeof (T);
                throw new AttributeNotFoundException($"Не найдено определение аттрибута {type} на {t}", type, t);
            }
            return res;
        }
    }
}