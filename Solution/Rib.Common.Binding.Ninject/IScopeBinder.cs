namespace Rib.Common.Binding.Ninject
{
    using global::Ninject.Syntax;
    using JetBrains.Annotations;
    using Rib.Common.Models.Binding;

    public interface IScopeBinder
    {
        [NotNull]
        IBindingNamedWithOrOnSyntax<object> InScope([NotNull] IBindingWhenInNamedWithOrOnSyntax<object> binded, [NotNull] BindingScope scope);
    }
}