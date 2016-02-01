namespace Rib.Common.Helpers.Expressions
{
    using System.Reflection;
    using JetBrains.Annotations;

    public interface IPropertyHelper
    {
        object Get([NotNull] PropertyInfo pi, [NotNull] object o);

        void Set([NotNull] PropertyInfo pi, [NotNull] object o, object val);
    }
}