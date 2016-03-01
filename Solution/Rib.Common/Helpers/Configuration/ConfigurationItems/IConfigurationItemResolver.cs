namespace Rib.Common.Helpers.Configuration.ConfigurationItems
{
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(ConfigurationItemResolver))]
    public interface IConfigurationItemResolver
    {
        [NotNull]
        ConfigurationItem Resolve([NotNull] string name);
    }
}