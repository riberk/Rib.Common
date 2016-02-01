namespace Rib.Common.Helpers.Metadata.Enums
{
    using System;
    using JetBrains.Annotations;

    public class EnumAttributeReader : IEnumAttributeReader
    {
        [NotNull] private readonly IAttributesReader _attributesReader;
        [NotNull] private readonly IEnumFieldReader _enumFieldReader;

        public EnumAttributeReader([NotNull] IAttributesReader attributesReader, [NotNull] IEnumFieldReader enumFieldReader)
        {
            if (attributesReader == null) throw new ArgumentNullException(nameof(attributesReader));
            if (enumFieldReader == null) throw new ArgumentNullException(nameof(enumFieldReader));
            _attributesReader = attributesReader;
            _enumFieldReader = enumFieldReader;
        }

        public TAttribute Attribute<TAttribute>(Enum e) where TAttribute : Attribute
        {
            return e == null ? null : _attributesReader.Read<TAttribute>(_enumFieldReader.Field(e));
        }

        public TAttribute Attribute<TAttribute, TEnum>(TEnum e) where TAttribute : Attribute where TEnum : struct
        {
            return _attributesReader.Read<TAttribute>(_enumFieldReader.Field(e));
        }

        public TAttribute AttributeSafe<TAttribute>(Enum e) where TAttribute : Attribute
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            return _attributesReader.ReadSafe<TAttribute>(_enumFieldReader.Field(e));
        }

        public TAttribute AttributeSafe<TAttribute, TEnum>(TEnum e) where TAttribute : Attribute where TEnum : struct
        {
            return _attributesReader.ReadSafe<TAttribute>(_enumFieldReader.Field(e));
        }
    }
}