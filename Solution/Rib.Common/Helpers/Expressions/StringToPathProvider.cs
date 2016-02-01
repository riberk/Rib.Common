namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using Rib.Common.Helpers.Cache;
    using JetBrains.Annotations;

    public class StringToPathProvider : IStringToPathProvider
    {
        [NotNull] private readonly ICacherFactory _cacherFactory;

        public StringToPathProvider([NotNull] ICacherFactory cacherFactory)
        {
            if (cacherFactory == null) throw new ArgumentNullException(nameof(cacherFactory));
            _cacherFactory = cacherFactory;
        }

        public IEnumerable<PropertyInfo> GetProperties(Type currentType, IEnumerable<string> propNames)
        {
            if (currentType == null) throw new ArgumentNullException(nameof(currentType));
            if (propNames == null) throw new ArgumentNullException(nameof(propNames));
            var cache = _cacherFactory.Create<PropertyInfo>(typeof (StringToPathProvider).FullName);
            foreach (var propName in propNames)
            {
                if (string.IsNullOrWhiteSpace(propName))
                {
                    throw new InvalidOperationException("Свойство не может быть пустым");
                }
                var res = cache.GetOrAdd($"{currentType.FullName}|{propName}", key =>
                {
                    // ReSharper disable once AccessToModifiedClosure
                    var property = currentType.GetProperty(propName);
                    if (property == null)
                    {
                        // ReSharper disable once AccessToModifiedClosure
                        throw new InvalidOperationException($"Свойство {propName} не найдено в типе {currentType}");
                    }
                    return property;
                });
                if (res == null)
                {
                    throw new InvalidOperationException("Cache returned null");
                }
                yield return res;
                currentType = res.PropertyType;
            }
        }
    }
}