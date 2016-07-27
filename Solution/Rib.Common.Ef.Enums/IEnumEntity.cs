namespace Rib.Common.Ef.Enums
{
    using System;
    using TsSoft.EntityRepository.Interfaces;

    /// <summary>Reflected of enum to the database</summary>
    /// <typeparam name="T">Enum type</typeparam>
    public interface IEnumEntity<T> : IEntityWithId<T>, IDeletable
            where T : struct, IComparable, IFormattable, IConvertible
    {
        /// <summary>Enum field name</summary>
        string Name { get; set; }

        /// <summary>Title of enum value</summary>
        string Title { get; set; }
    }
}