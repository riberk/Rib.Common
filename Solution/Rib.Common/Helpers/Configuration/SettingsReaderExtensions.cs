namespace Rib.Common.Helpers.Configuration
{
    using System;
    using JetBrains.Annotations;

    public static class SettingsReaderExtensions
    {
        [NotNull]
        public static string ReadSafe<T>([NotNull] this ISettingsReader<T> settingsReader, [NotNull] T name) where T : class
        {
            if (settingsReader == null) throw new ArgumentNullException(nameof(settingsReader));
            if (name == null) throw new ArgumentNullException(nameof(name));
            var res = settingsReader.Read(name);
            if (string.IsNullOrWhiteSpace(res))
            {
                throw new InvalidOperationException($"Отсутствует параметр конфигурации {name} или его значение пусто");
            }
            return res;
        }

        public static int? ReadInt<T>([NotNull] this ISettingsReader<T> settingsReader, [NotNull] T name) where T : class
        {
            if (settingsReader == null) throw new ArgumentNullException(nameof(settingsReader));
            if (name == null) throw new ArgumentNullException(nameof(name));
            var res = settingsReader.Read(name);
            if (string.IsNullOrWhiteSpace(res))
            {
                return null;
            }
            int i;
            if (!int.TryParse(res, out i))
            {
                throw new InvalidCastException($"Невозможно привести значение конфигурации: ожидается число или null, актуальное значение {res}");
            }
            return i;
        }

        public static int ReadSafeInt<T>([NotNull] this ISettingsReader<T> settingsReader, [NotNull] T name) where T : class
        {
            if (settingsReader == null) throw new ArgumentNullException(nameof(settingsReader));
            if (name == null) throw new ArgumentNullException(nameof(name));
            var res = settingsReader.ReadSafe(name);
            int i;
            if (!int.TryParse(res, out i))
            {
                throw new InvalidCastException($"Невозможно привести значение конфигурации: ожидается число, актуальное значение {res}");
            }
            return i;
        }

        public static bool? ReadBool<T>([NotNull] this ISettingsReader<T> settingsReader, [NotNull] T name) where T : class
        {
            if (settingsReader == null) throw new ArgumentNullException(nameof(settingsReader));
            if (name == null) throw new ArgumentNullException(nameof(name));
            var res = settingsReader.Read(name);
            if (string.IsNullOrWhiteSpace(res))
            {
                return null;
            }
            bool i;
            if (!bool.TryParse(res, out i))
            {
                throw new InvalidCastException(
                    $"Невозможно привести значение конфигурации {name}: ожидается булево или null, актуальное значение {res}");
            }
            return i;
        }

        public static bool ReadSafeBool<T>([NotNull] this ISettingsReader<T> settingsReader, [NotNull] T name) where T : class
        {
            if (settingsReader == null) throw new ArgumentNullException(nameof(settingsReader));
            if (name == null) throw new ArgumentNullException(nameof(name));
            var res = settingsReader.ReadSafe(name);
            bool i;
            if (!bool.TryParse(res, out i))
            {
                throw new InvalidCastException($"Невозможно привести значение конфигурации: ожидается булево, актуальное значение {res}");
            }
            return i;
        }
    }
}