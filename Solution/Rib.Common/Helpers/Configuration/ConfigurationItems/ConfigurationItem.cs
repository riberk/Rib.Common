namespace Rib.Common.Helpers.Configuration.ConfigurationItems
{
    using System;
    using JetBrains.Annotations;

    public class ConfigurationItem
    {
        public ConfigurationItem([NotNull] string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Name = name;
        }

        [NotNull]
        public string Name { get; }

        /// <summary>
        ///     ���������� ������, �������������� ������� ������.
        /// </summary>
        /// <returns>
        ///     ������, �������������� ������� ������.
        /// </returns>
        public override string ToString()
        {
            return $"Name: {Name}";
        }

        public static implicit operator string(ConfigurationItem item)
        {
            return item?.Name;
        }
    }
}