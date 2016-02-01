namespace Rib.Common.Helpers.Cache
{
    using JetBrains.Annotations;

    /// <summary>
    ///     Чистильщик кеша
    /// </summary>
    public interface ICacheCleaner
    {
        /// <summary>
        ///     Очистить все кеши
        /// </summary>
        void Clean();

        /// <summary>
        ///     Очистить итем кеша по имени
        /// </summary>
        /// <param name="fullName">Полный ключ итема</param>
        void Clean([NotNull] string fullName);
    }
}