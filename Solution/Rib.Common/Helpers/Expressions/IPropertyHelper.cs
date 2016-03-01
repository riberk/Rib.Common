namespace Rib.Common.Helpers.Expressions
{
    using System.Reflection;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(PropertyHelper))]
    public interface IPropertyHelper
    {
        object Get([NotNull] PropertyInfo pi, [NotNull] object o);

        void Set([NotNull] PropertyInfo pi, [NotNull] object o, object val);
    }
}