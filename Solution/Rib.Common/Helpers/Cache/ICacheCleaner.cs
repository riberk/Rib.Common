namespace Rib.Common.Helpers.Cache
{
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    /// <summary>
    ///     Чистильщик кеша
    /// </summary>
    [BindTo(typeof(CacheCleaner))]
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