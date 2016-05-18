namespace Rib.Common.Models.Interfaces
{
    /// <summary>
    ///     Сущность, допускающая сортировку
    /// </summary>
    public interface IOrderedEntity
    {
        /// <summary>
        ///     Порядок
        /// </summary>
        int Order { get; set; }
    }
}