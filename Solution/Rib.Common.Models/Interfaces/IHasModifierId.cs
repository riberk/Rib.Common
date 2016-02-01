namespace Rib.Common.Models.Interfaces
{
    /// <summary>
    ///     Сущность, обладающая идентификатором последнего изменившего
    /// </summary>
    public interface IHasModifierId<T> where T: struct 
    {
        /// <summary>
        ///     Ид последнего изменившего пользователя
        /// </summary>
        T? ModifierId { get; set; }
    }
}