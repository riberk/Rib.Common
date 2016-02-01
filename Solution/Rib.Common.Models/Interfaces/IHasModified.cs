namespace Rib.Common.Models.Interfaces
{
    using System;

    /// <summary>
    ///     Сущность, обладающая датой последнего изменения
    /// </summary>
    public interface IHasModified
    {
        /// <summary>
        ///     Дата последнего изменения
        /// </summary>
        DateTime Modified { get; set; }
    }
}