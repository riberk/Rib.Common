namespace Rib.Common.Models.Metadata
{
    /// <summary>
    ///     Резолвер чего-то, зависящего от контекста
    /// </summary>
    public interface IContextPropertyResolver
    {
        /// <summary>
        ///     Получить
        /// </summary>
        object Resolve(object currentValue);
    }
}