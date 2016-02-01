namespace Rib.Common.Helpers.Configuration
{
    using JetBrains.Annotations;

    public interface ISettingsWriter
    {
        void Write([NotNull] string name, string value);
    }
}