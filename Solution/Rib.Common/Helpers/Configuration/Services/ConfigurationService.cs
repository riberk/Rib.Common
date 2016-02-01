namespace Rib.Common.Helpers.Configuration.Services
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.Linq;
    using JetBrains.Annotations;
    using Rib.Common.Helpers.Configuration.ConfigurationItems;
    using Rib.Common.Helpers.Configuration.SettingsManagers;
    using Rib.Common.Helpers.Metadata;
    using Rib.Common.Models.Configuration;

    internal class ConfigurationService : IConfigurationService
    {
        [NotNull] private readonly IAttributesReader _attributesReader;
        [NotNull] private readonly IConfigurationItemsHelper _configurationItemsHelper;
        [NotNull] private readonly ICanEditItemChecker _canEditItemChecker;
        [NotNull] private readonly IConfigurationReader _configurationReader;
        [NotNull] private readonly IConfigurationTypeResolver _configurationTypeResolver;

        public ConfigurationService([NotNull] IAttributesReader attributesReader,
                                    [NotNull] IConfigurationReader configurationReader,
                                    [NotNull] IConfigurationTypeResolver configurationTypeResolver,
                                    [NotNull] IConfigurationItemsHelper configurationItemsHelper,
                                    [NotNull] ICanEditItemChecker canEditItemChecker)
        {
            if (attributesReader == null) throw new ArgumentNullException(nameof(attributesReader));
            if (configurationReader == null) throw new ArgumentNullException(nameof(configurationReader));
            if (configurationTypeResolver == null) throw new ArgumentNullException(nameof(configurationTypeResolver));
            if (configurationItemsHelper == null) throw new ArgumentNullException(nameof(configurationItemsHelper));
            if (canEditItemChecker == null) throw new ArgumentNullException(nameof(canEditItemChecker));
            _attributesReader = attributesReader;
            _configurationReader = configurationReader;
            _configurationTypeResolver = configurationTypeResolver;
            _configurationItemsHelper = configurationItemsHelper;
            _canEditItemChecker = canEditItemChecker;
        }

        public IReadOnlyCollection<ConfigurationItemGroupTableModel> Read()
        {
            return _configurationItemsHelper.GroupedTypes(_configurationTypeResolver.Resolve())
                                            .Select(ReadGroup)
                                            .ToList();
        }

        private ConfigurationItemGroupTableModel ReadGroup([NotNull] Type t)
        {
            var description = _attributesReader.ReadSafe<DescriptionAttribute>(t).Description;
            if (string.IsNullOrWhiteSpace(description))
            {
                throw new InvalidOperationException($"Отсутствует описание в атрибуте на {t}");
            }
            var items = ReadItems(t);
            var groups = _configurationItemsHelper.GroupedTypes(t).Select(ReadGroup).ToList();
            return new ConfigurationItemGroupTableModel(description)
            {
                Groups = groups,
                Items = items
            };
        }

        [NotNull, ItemNotNull]
        private IReadOnlyCollection<ConfigurationItemTableModel> ReadItems([NotNull] Type t)
        {
            return
                    _configurationItemsHelper
                            .Items(t)
                            .Select(info =>
                            {
                                var fieldValue = info.GetValue(null);
                                if (fieldValue == null)
                                {
                                    throw new InvalidOperationException($"Значение статического свойства {info} равно null");
                                }
                                var configInfo = (ConfigurationItem) fieldValue;
                                var value = _configurationReader.Read(configInfo);
                                var desc = _attributesReader.ReadSafe<DescriptionAttribute>(info).Description;
                                return new ConfigurationItemTableModel(configInfo, value, desc, _canEditItemChecker.CanEdit(info, configInfo));
                            }).ToList();
        }
    }
}