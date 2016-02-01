namespace Rib.Common.Models.Interfaces
{
    /// <summary>
    ///     Сущность, обладающая ссылкой на последнего изменившего ее пользователя
    /// </summary>
    /// <typeparam name="TLink">Тип ссылки</typeparam>
    public interface IHasModifierLink<TLink>
    {
        /// <summary>
        ///     Ссылка на последнего изменившего сущность пользователя
        /// </summary>
        TLink Modifier { get; set; }
    }
}