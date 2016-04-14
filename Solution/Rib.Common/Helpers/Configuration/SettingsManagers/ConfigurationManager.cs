namespace Rib.Common.Helpers.Configuration.SettingsManagers
{
    using System;
    using JetBrains.Annotations;
    using Rib.Common.Helpers.Configuration.ConfigurationItems;
    using Rib.Common.Models.Metadata;

    [BindFrom(typeof(IConfigurationReader), typeof(IConfigurationWriter), typeof(IConfigurationManager))]
    internal class ConfigurationManager : IConfigurationManager
    {
        [NotNull] private readonly IConfigurationItemResolver _configurationItemResolver;
        [NotNull] private readonly ISettingsReaderFactory _readerFactory;
        [NotNull] private readonly ISettingsWriterFactory _settingsWriterFactory;

        public ConfigurationManager(
            [NotNull] ISettingsReaderFactory readerFactory,
            [NotNull] ISettingsWriterFactory settingsWriterFactory,
            [NotNull] IConfigurationItemResolver configurationItemResolver)
        {
            if (readerFactory == null) throw new ArgumentNullException(nameof(readerFactory));
            if (settingsWriterFactory == null) throw new ArgumentNullException(nameof(settingsWriterFactory));
            if (configurationItemResolver == null) throw new ArgumentNullException(nameof(configurationItemResolver));
            _readerFactory = readerFactory;
            _settingsWriterFactory = settingsWriterFactory;
            _configurationItemResolver = configurationItemResolver;
        }

        public string Read(ConfigurationItem item)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            return _readerFactory.Create(item).Read(item.Name);
        }

        public void Write(string name, string value)
        {
            if (name == null) throw new ArgumentNullException(nameof(name));
            _settingsWriterFactory.Create(_configurationItemResolver.Resolve(name)).Write(name, value);
        }
    }
}