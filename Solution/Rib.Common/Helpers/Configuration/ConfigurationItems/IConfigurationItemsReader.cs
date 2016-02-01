namespace Rib.Common.Helpers.Configuration.ConfigurationItems
{
    using System.Collections.Generic;
    using System.Reflection;
    using JetBrains.Annotations;

    public interface IConfigurationItemsReader
    {
        [NotNull, ItemNotNull]
        IEnumerable<ConfigurationItem> ReadAll();

        [NotNull, ItemNotNull]
        IDictionary<FieldInfo, ConfigurationItem> ReadAllWithFields();
    }
}