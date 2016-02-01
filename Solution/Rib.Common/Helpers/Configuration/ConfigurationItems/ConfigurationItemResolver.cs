namespace Rib.Common.Helpers.Configuration.ConfigurationItems
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using JetBrains.Annotations;

    internal class ConfigurationItemResolver : IConfigurationItemResolver
    {
        [NotNull] private readonly IConfigurationItemsReader _configurationItemsReader;
        [NotNull, ItemNotNull] private readonly Lazy<IDictionary<string, ConfigurationItem>> _lazyValues;

        public ConfigurationItemResolver([NotNull] IConfigurationItemsReader configurationItemsReader)
        {
            if (configurationItemsReader == null) throw new ArgumentNullException(nameof(configurationItemsReader));
            _configurationItemsReader = configurationItemsReader;
            _lazyValues = new Lazy<IDictionary<string, ConfigurationItem>>(AllItems);
        }

        public ConfigurationItem Resolve(string name)
        {
            ConfigurationItem item;
            if (!_lazyValues.Value.TryGetValue(name, out item))
            {
                throw new KeyNotFoundException($"Не найден параметр конфигурации с именем {name}");
            }
            return item;
        }

        private IDictionary<string, ConfigurationItem> AllItems()
        {
            return _configurationItemsReader.ReadAll().ToDictionary(x => x.Name, x => x);
        }
    }
}