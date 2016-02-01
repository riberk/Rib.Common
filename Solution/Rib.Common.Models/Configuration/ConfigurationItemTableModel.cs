namespace Rib.Common.Models.Configuration
{
    public class ConfigurationItemTableModel
    {
        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="ConfigurationItemTableModel" />.
        /// </summary>
        public ConfigurationItemTableModel(string name, string value, string description, bool canChange)
        {
            Name = name;
            Value = value;
            CanChange = canChange;
            Description = description;
        }

        public string Name { get; }
        public string Value { get; }
        public string Description { get; }
        public bool CanChange { get; }
    }
}