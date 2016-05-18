namespace Rib.Common.Application.Web.WebApi.Filters
{
    using System;
    using JetBrains.Annotations;
    using Rib.Common.Application.Web.WebApi.Helpers;
    using Rib.Common.Helpers.Configuration;
    using Rib.Common.Helpers.Configuration.ConfigurationItems;
    using Rib.Common.Helpers.Configuration.SettingsManagers;

    public abstract class SettingsIncludeErrorDetailsResolver : IIncludeErrorDetailsResolver
    {
        [NotNull] private readonly IConfigurationReader _configurationReader;

        protected SettingsIncludeErrorDetailsResolver([NotNull] IConfigurationReader configurationReader)
        {
            if (configurationReader == null) throw new ArgumentNullException(nameof(configurationReader));
            _configurationReader = configurationReader;
        }

        [NotNull]
        protected abstract ConfigurationItem Item { get; }

        public bool? IncludeErrorDetails => _configurationReader.ReadBool(Item);
    }
}