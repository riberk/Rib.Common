namespace Rib.Common.Helpers.Configuration.SettingsManagers
{
    using JetBrains.Annotations;
    using Rib.Common.Helpers.Configuration.ConfigurationItems;

    public interface ISettingsWriterFactory
    {
        [NotNull]
        ISettingsWriter Create([NotNull] ConfigurationItem item);
    }
}