namespace Rib.Tests.Ninject
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;
    using Rib.Common.Helpers.Metadata.Enums;

    public class EnumsChecker
    {
        [NotNull]
        private readonly IEnumFieldReader _enumFieldReader;
        [NotNull]
        private readonly HashSet<Type> _enums;
        [NotNull]
        private readonly List<string> _errors;

        public EnumsChecker(IEnumerable<Assembly> assemblies, [NotNull] IEnumFieldReader enumFieldReader)
        {
            if (enumFieldReader == null) throw new ArgumentNullException(nameof(enumFieldReader));
            _enumFieldReader = enumFieldReader;
            _errors = new List<string>();
            _enums = new HashSet<Type>(assemblies.SelectMany(t => t.GetTypes()).Where(x => x.IsEnum));
        }

        [NotNull]
        public IReadOnlyCollection<string> Errors => _errors;

        public EnumsChecker CheckDescription()
        {
            foreach (var @enum in _enums)
            {
                var typeDescAttr = @enum.GetCustomAttribute<DescriptionAttribute>();
                if (string.IsNullOrWhiteSpace(typeDescAttr?.Description))
                {
                    _errors.Add($"Not found description attribute on {@enum} or it is empty");
                }
                var enumValues = Enum.GetValues(@enum).Cast<Enum>();
                var enumerable = enumValues
                        .Where(enumValue => string.IsNullOrWhiteSpace(_enumFieldReader.Field(enumValue)
                                                                                      .GetCustomAttribute<DescriptionAttribute>()?
                                                                                      .Description));
                foreach (var enumValue in enumerable)
                {
                    _errors.Add($"Not found description attribute on {@enum} on {enumValue} or it is empty");
                }
            }
            return this;
        }

        public EnumsChecker CheckIdentityValues()
        {
            foreach (var @enum in _enums)
            {
                var duplicateValues = Enum.GetValues(@enum).Cast<Enum>().Select(e => new
                {
                    v = Convert.ChangeType(e, e.GetTypeCode()),
                    ev = e
                }).GroupBy(e => e.v).Where(e => e.Count() > 1);
                foreach (var duplicateValue in duplicateValues)
                {
                    _errors.Add($"Duplicate values {duplicateValue.Key} on {@enum}: {string.Join(", ", duplicateValue)}");
                }
            }
            return this;
        }
    }
}