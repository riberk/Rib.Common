namespace Rib.Common.Models.Metadata
{
    using System;
    using JetBrains.Annotations;

    [AttributeUsage(AttributeTargets.Enum)]
    public class ClientEnumAttribute : Attribute
    {
        public readonly string FriendlyName;

        public ClientEnumAttribute([NotNull] string friendlyName)
        {
            if (friendlyName == null) throw new ArgumentNullException(nameof(friendlyName));
            FriendlyName = friendlyName;
        }
    }
}