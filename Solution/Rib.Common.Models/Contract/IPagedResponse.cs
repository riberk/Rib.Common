namespace Rib.Common.Models.Contract
{
    using System.Collections.Generic;
    using JetBrains.Annotations;

    public interface IPagedResponse<out T>
    {
        [NotNull]
        IReadOnlyCollection<T> Value { get; }

        int Count { get; }
    }
}