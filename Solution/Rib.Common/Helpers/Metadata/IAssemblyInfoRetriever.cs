namespace Rib.Common.Helpers.Metadata
{
    using System.Reflection;
    using Rib.Common.Models.Metadata;
    using JetBrains.Annotations;

    /// <summary>
    ///     Получатель информации о сборке
    /// </summary>
    public interface IAssemblyInfoRetriever
    {
        /// <summary>
        ///     Получить информацию о сборке
        /// </summary>
        /// <param name="assembly">Сборка</param>
        /// <returns>Информация о сборке</returns>
        IAssemblyInfo Retrieve([NotNull] Assembly assembly);
    }
}