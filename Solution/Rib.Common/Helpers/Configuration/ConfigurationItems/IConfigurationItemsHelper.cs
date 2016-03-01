namespace Rib.Common.Helpers.Configuration.ConfigurationItems
{
    using System;
    using System.Collections.Generic;
    using System.Reflection;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(ConfigurationItemsHelper))]
    public interface IConfigurationItemsHelper
    {
        [NotNull, ItemNotNull]
        IEnumerable<Type> GroupedTypes([NotNull] Type t);

        [NotNull, ItemNotNull]
        IEnumerable<FieldInfo> Items([NotNull] Type t);

        [NotNull]
        FieldInfo Item([NotNull] Type t, [NotNull] string name);
    }
}