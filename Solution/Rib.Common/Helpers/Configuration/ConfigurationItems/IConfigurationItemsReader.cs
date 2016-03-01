namespace Rib.Common.Helpers.Configuration.ConfigurationItems
{
    using System.Collections.Generic;
    using System.Reflection;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(ConfigurationItemsReader))]
    public interface IConfigurationItemsReader
    {
        [NotNull, ItemNotNull]
        IEnumerable<ConfigurationItem> ReadAll();

        [NotNull, ItemNotNull]
        IDictionary<FieldInfo, ConfigurationItem> ReadAllWithFields();
    }
}