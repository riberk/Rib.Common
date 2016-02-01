namespace Rib.Common.Helpers.Metadata.Enums
{
    using System;
    using System.ComponentModel;
    using JetBrains.Annotations;

    public static class EnumAttributeReaderExtensions
    {
        public static string Description<TEnum>([NotNull] this IEnumAttributeReader enumAttributeReader, TEnum e) where TEnum : struct
        {
            if (enumAttributeReader == null) throw new ArgumentNullException(nameof(enumAttributeReader));
            return enumAttributeReader.Attribute<DescriptionAttribute, TEnum>(e)?.Description;
        }

        public static string Description([NotNull] this IEnumAttributeReader enumAttributeReader, Enum e)
        {
            if (enumAttributeReader == null) throw new ArgumentNullException(nameof(enumAttributeReader));
            return enumAttributeReader.Attribute<DescriptionAttribute>(e)?.Description;
        }

        public static string DescriptionSafe<TEnum>([NotNull] this IEnumAttributeReader enumAttributeReader, TEnum e) where TEnum : struct
        {
            if (enumAttributeReader == null) throw new ArgumentNullException(nameof(enumAttributeReader));
            var d = enumAttributeReader.AttributeSafe<DescriptionAttribute, TEnum>(e).Description;
            if (string.IsNullOrWhiteSpace(d))
            {
                throw new InvalidOperationException($"Description on {e} null or empty");
            }
            return d;
        }

        public static string DescriptionSafe([NotNull] this IEnumAttributeReader enumAttributeReader, [NotNull] Enum e)
        {
            if (enumAttributeReader == null) throw new ArgumentNullException(nameof(enumAttributeReader));
            if (e == null) throw new ArgumentNullException(nameof(e));
            var d = enumAttributeReader.AttributeSafe<DescriptionAttribute>(e).Description;
            if (string.IsNullOrWhiteSpace(d))
            {
                throw new InvalidOperationException($"Description on {e} null or empty");
            }
            return d;
        }
    }
}