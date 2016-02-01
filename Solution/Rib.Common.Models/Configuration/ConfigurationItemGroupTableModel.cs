namespace Rib.Common.Models.Configuration
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    public class ConfigurationItemGroupTableModel
    {
        public ConfigurationItemGroupTableModel([NotNull] string description)
        {
            if (description == null) throw new ArgumentNullException(nameof(description));
            Description = description;
            Items = new HashSet<ConfigurationItemTableModel>();
            Groups = new HashSet<ConfigurationItemGroupTableModel>();
        }

        [NotNull]
        public string Description { get; }

        [NotNull, ItemNotNull]
        public IReadOnlyCollection<ConfigurationItemTableModel> Items { get; set; }

        [NotNull, ItemNotNull]
        public IReadOnlyCollection<ConfigurationItemGroupTableModel> Groups { get; set; }
    }
}