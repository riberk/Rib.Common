namespace Rib.Common.Application.Rest.Infrastructure
{
    using System.Collections.Generic;
    using Rib.Common.Models.Metadata;
    using TsSoft.Expressions.OrderBy;

    /// <summary>
    ///     Создает правила сортировки для сущности
    /// </summary>
    /// <typeparam name="T">Тип сущности</typeparam>
    [BindTo(typeof(OrderCreator<>))]
    public interface IOrderCreator<T>
            where T : class
    {
        /// <summary>
        ///     Создать правила сортировки из словаря
        /// </summary>
        /// <param name="orderProperties">
        ///     Словарь строка->булево значение,
        ///     где строка - paramName.Property1.Property2
        ///     где paramName - имя параметра типа {T} (ни на что не влияет, сделано для удобства)
        ///     Property1.Property2 - путь к свойству для сортровки
        ///     Булево значение - descending
        /// </param>
        /// <returns>Перечисление правил сортировки сущности</returns>
        IEnumerable<IOrderByClause<T>> Create(IReadOnlyCollection<KeyValuePair<string, bool>> orderProperties);
    }
}