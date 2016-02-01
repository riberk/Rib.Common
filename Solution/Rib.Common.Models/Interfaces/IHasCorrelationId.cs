namespace Rib.Common.Models.Interfaces
{
    using System;

    /// <summary>
    ///     Сущность, имеющая ид кореляции
    /// </summary>
    public interface IHasCorrelationId
    {
        /// <summary>
        ///     Ид кореляции
        /// </summary>
        Guid CorrelationId { get; set; }
    }
}