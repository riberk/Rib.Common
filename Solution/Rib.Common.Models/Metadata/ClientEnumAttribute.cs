namespace Rib.Common.Models.Metadata
{
    using System;

    [AttributeUsage(AttributeTargets.Enum)]
    public class ClientEnumAttribute : Attribute
    {
        public readonly string FriendlyName;

        public ClientEnumAttribute(string friendlyName)
        {
            FriendlyName = friendlyName;
        }
    }
}