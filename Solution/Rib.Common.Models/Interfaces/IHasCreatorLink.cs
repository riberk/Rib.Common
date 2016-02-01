namespace Rib.Common.Models.Interfaces
{
    /// <summary>
    ///     Сущность, обладающая ссылкой на создавшего ее пользователя
    /// </summary>
    /// <typeparam name="TLink">Тип ссылки</typeparam>
    public interface IHasCreatorLink<TLink>
    {
        /// <summary>
        ///     Ссылка на создавшего сущность пользователя
        /// </summary>
        TLink Creator { get; set; }
    }
}