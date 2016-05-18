namespace Rib.Common.Application.Web.WebApi.Filters
{
    using System;
    using JetBrains.Annotations;
    using Rib.Common.Helpers.Configuration.ConfigurationItems;
    using Rib.Common.Helpers.Configuration.SettingsManagers;

    public class SettingsIncludeErrorDetailsResolverImpl : SettingsIncludeErrorDetailsResolver
    {
        public SettingsIncludeErrorDetailsResolverImpl([NotNull] IConfigurationReader configurationReader,
                                                       [NotNull] ConfigurationItem item)
                : base(configurationReader)
        {
            if (item == null) throw new ArgumentNullException(nameof(item));
            Item = item;
        }

        protected override ConfigurationItem Item { get; }
    }
}