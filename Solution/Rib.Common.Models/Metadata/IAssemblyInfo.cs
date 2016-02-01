namespace Rib.Common.Models.Metadata
{
    using System;

    /// <summary>
    ///     Информация о сборке
    /// </summary>
    public interface IAssemblyInfo
    {
        /// <summary>
        ///     Версия
        /// </summary>
        string Version { get; }

        /// <summary>
        ///     Дата и время сборки
        /// </summary>
        DateTime BuildAt { get; }
    }
}