namespace Rib.Common.Helpers.Configuration
{
    using System;
    using System.Configuration;

    internal class AppSettingsManager : ISettingsManager
    {
        public string Read(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name), $"{nameof(name)} is null or white space");
            return ConfigurationManager.AppSettings[name];
        }

        public void Write(string name, string value)
        {
            throw new NotSupportedException("Ќевозможно произвести запись в .config приложени€");
        }
    }
}