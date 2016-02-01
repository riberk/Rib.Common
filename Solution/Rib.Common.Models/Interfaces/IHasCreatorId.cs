namespace Rib.Common.Models.Interfaces
{
    /// <summary>
    ///     Сущность, обладающая идентификатором создавшего пользователя
    /// </summary>
    public interface IHasCreatorId<T> where T : struct 
    {
        /// <summary>
        ///     Ид последнего создавшего пользователя
        /// </summary>
        T? CreatorId { get; set; }
    }
}