namespace Rib.Common.Models.Contract
{
    using System.Collections.Generic;

    public class PagedResponse<T> : IPagedResponse<T>
    {
        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="T:System.Object" />.
        /// </summary>
        public PagedResponse(IReadOnlyCollection<T> value, int count)
        {
            Value = value ?? new T[0];
            Count = count;
        }

        public IReadOnlyCollection<T> Value { get; }

        public int Count { get; }
    }
}