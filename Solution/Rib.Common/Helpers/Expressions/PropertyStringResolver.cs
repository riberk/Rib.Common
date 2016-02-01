namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;

    public class PropertyStringResolver : IPropertyStringResolver
    {
        [NotNull] private readonly IStringToPathProvider _stringToPathProvider;

        public PropertyStringResolver([NotNull] IStringToPathProvider stringToPathProvider)
        {
            if (stringToPathProvider == null) throw new ArgumentNullException(nameof(stringToPathProvider));
            _stringToPathProvider = stringToPathProvider;
        }

        public IEnumerable<PropertyInfo> GetProperties(Type currentType, string propPath)
        {
            if (currentType == null) throw new ArgumentNullException(nameof(currentType));
            if (propPath == null) throw new ArgumentNullException(nameof(propPath));
            var res = propPath.Split(new[] {'.'}, StringSplitOptions.None);
            if (res.Length <= 1)
            {
                throw new InvalidOperationException($"Не удалось распарсить строку на составляющие пути. Исходная строка: {propPath}");
            }
            return _stringToPathProvider.GetProperties(currentType, res.Skip(1));
        }
    }
}