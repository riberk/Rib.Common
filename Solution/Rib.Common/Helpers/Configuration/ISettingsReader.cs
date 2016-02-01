namespace Rib.Common.Helpers.Configuration
{
    using JetBrains.Annotations;

    public interface ISettingsReader<in T> where T : class
    {
        string Read([NotNull] T name);
    }

    public interface ISettingsReader : ISettingsReader<string>
    {
    }
}