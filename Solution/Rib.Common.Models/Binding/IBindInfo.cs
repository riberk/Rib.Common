namespace Rib.Common.Models.Binding
{
    using System;
    using System.Collections.Generic;

    public interface IBindInfo
    {
        BindingScope Scope { get; }
        IReadOnlyCollection<Type>  From { get; }
        Type To { get; }
        string Name { get; }
    }
}