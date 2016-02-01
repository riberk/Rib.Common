namespace Rib.Common.Helpers.Configuration
{
    using System;
    using JetBrains.Annotations;

    public static class SettingsWriterExtensions
    {
        public static void Write<T>([NotNull] this ISettingsWriter settingsWriter, [NotNull] string name, T value) where T : struct
        {
            if (settingsWriter == null) throw new ArgumentNullException(nameof(settingsWriter));
            if (name == null) throw new ArgumentNullException(nameof(name));
            settingsWriter.Write(name, value.ToString());
        }

        public static void Write<T>([NotNull] this ISettingsWriter settingsWriter, [NotNull] string name, T? value) where T : struct
        {
            if (settingsWriter == null) throw new ArgumentNullException(nameof(settingsWriter));
            if (name == null) throw new ArgumentNullException(nameof(name));
            settingsWriter.Write(name, value?.ToString());
        }
    }
}