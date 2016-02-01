namespace Rib.Common.Helpers.Metadata.Enums
{
    using System;
    using System.Reflection;
    using Rib.Common.Helpers.Cache;
    using Rib.Common.Models.Exceptions;
    using Rib.Common.Models.Metadata;
    using JetBrains.Annotations;

    internal class EnumReader : IEnumReader
    {
        [NotNull] private readonly IEnumAttributeReader _attributeReader;
        [NotNull] private readonly ICacherFactory _cacherFactory;

        public EnumReader([NotNull] IEnumAttributeReader attributeReader, [NotNull] ICacherFactory cacherFactory)
        {
            if (attributeReader == null) throw new ArgumentNullException(nameof(attributeReader));
            if (cacherFactory == null) throw new ArgumentNullException(nameof(cacherFactory));
            _attributeReader = attributeReader;
            _cacherFactory = cacherFactory;
        }

        public EnumModel Read([NotNull] Enum e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            return _cacherFactory.Create<EnumModel>().GetOrAdd($"{e.GetType()}|{e}", s =>
            {
                var val = Convert.ChangeType(e, e.GetTypeCode());
                var description = _attributeReader.Description(e);
                var res = (EnumModel)typeof (EnumModel<>).MakeGenericType(e.GetType())
                                    .GetConstructor(new[] {e.GetType(), typeof (string), typeof (object)})
                                    .Invoke(new[] {e, description, val});
                return res;
            });
        }

        public EnumModel<TEnum> Read<TEnum>(TEnum e) where TEnum : struct
        {
            var @enum = (Enum) (object) e;
            var readed = Read(@enum);
            var converted = readed as EnumModel<TEnum>;
            if (converted == null)
            {
                throw new InvalidCastException($"Could not cast {readed.GetType()} to {e.GetType()}");
            }
            return converted;
        }
    }
}