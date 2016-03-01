namespace Rib.Common.Helpers.Configuration.Services
{
    using System.Collections.Generic;
    using JetBrains.Annotations;
    using Rib.Common.Models.Configuration;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(ConfigurationService))]
    public interface IConfigurationService
    {
        [NotNull, ItemNotNull]
        IReadOnlyCollection<ConfigurationItemGroupTableModel> Read();
    }
}