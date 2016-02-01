namespace Rib.Common.Helpers.Configuration.Services
{
    using System.Reflection;
    using Rib.Common.Helpers.Configuration.ConfigurationItems;

    public interface ICanEditItemChecker
    {
        bool CanEdit(FieldInfo info, ConfigurationItem item);
    }
}