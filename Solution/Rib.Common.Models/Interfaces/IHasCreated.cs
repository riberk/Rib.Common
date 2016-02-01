namespace Rib.Common.Models.Interfaces
{
    using System;

    /// <summary>
    ///     Сущность, обладающая датой создания
    /// </summary>
    public interface IHasCreated
    {
        /// <summary>
        ///     Дата создания
        /// </summary>
        DateTime Created { get; set; }
    }
}