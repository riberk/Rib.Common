namespace Rib.Common.Helpers.Configuration.ConfigurationItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using JetBrains.Annotations;
    using Rib.Common.Helpers.Configuration.Services;

    internal class ConfigurationItemsReader : IConfigurationItemsReader
    {
        [NotNull] private readonly IConfigurationItemsHelper _configurationItemsHelper;
        [NotNull] private readonly IConfigurationTypeResolver _configurationTypeResolver;

        public ConfigurationItemsReader(
                [NotNull] IConfigurationTypeResolver configurationTypeResolver,
                [NotNull] IConfigurationItemsHelper configurationItemsHelper)
        {
            if (configurationTypeResolver == null) throw new ArgumentNullException(nameof(configurationTypeResolver));
            if (configurationItemsHelper == null) throw new ArgumentNullException(nameof(configurationItemsHelper));
            _configurationTypeResolver = configurationTypeResolver;
            _configurationItemsHelper = configurationItemsHelper;
        }

        public IEnumerable<ConfigurationItem> ReadAll()
        {
            return SelectKeyValues().Select(x => x.Value);
        }

        public IDictionary<FieldInfo, ConfigurationItem> ReadAllWithFields()
        {
            return SelectKeyValues()
                    .ToDictionary(x => x.Key, x => x.Value);
        }

        [NotNull, ItemNotNull]
        private IEnumerable<KeyValuePair<FieldInfo, ConfigurationItem>> SelectKeyValues()
        {
            return _configurationItemsHelper
                .GroupedTypes(_configurationTypeResolver.Resolve())
                .SelectMany(ReadItems);
        }

        [NotNull, ItemNotNull]
        private IEnumerable<KeyValuePair<FieldInfo, ConfigurationItem>> ReadItems([NotNull] Type type)
        {
            var items =
                    _configurationItemsHelper.Items(type).Select(f => new KeyValuePair<FieldInfo, ConfigurationItem>(f, (ConfigurationItem) f.GetValue(null)));
            var nestedItems = _configurationItemsHelper.GroupedTypes(type).SelectMany(ReadItems);
            return items.Concat(nestedItems);
        }
    }
}