namespace Rib.Common.Helpers.Configuration.SettingsManagers
{
    using JetBrains.Annotations;
    using Rib.Common.Helpers.Configuration.ConfigurationItems;

    public interface ISettingsReaderFactory
    {
        [NotNull]
        ISettingsReader Create([NotNull] ConfigurationItem item);
    }
}