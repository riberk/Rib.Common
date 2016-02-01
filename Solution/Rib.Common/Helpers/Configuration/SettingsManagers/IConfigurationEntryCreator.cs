namespace Rib.Common.Helpers.Configuration.SettingsManagers
{
    using JetBrains.Annotations;

    public interface IConfigurationEntryCreator<out TConfigurationEntry>
    {
        [NotNull]
        TConfigurationEntry Create(string name, string value);
    }
}