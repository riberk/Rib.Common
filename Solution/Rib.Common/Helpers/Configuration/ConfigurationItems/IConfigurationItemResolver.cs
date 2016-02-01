namespace Rib.Common.Helpers.Configuration.ConfigurationItems
{
    using JetBrains.Annotations;

    public interface IConfigurationItemResolver
    {
        [NotNull]
        ConfigurationItem Resolve([NotNull] string name);
    }
}