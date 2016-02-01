namespace Rib.Common.Helpers.Metadata.Enums
{
    using System;
    using System.Reflection;
    using Rib.Common.Helpers.Cache;
    using JetBrains.Annotations;

    /// <summary>
    ///     Реализация читателя полей по значениям енума
    /// </summary>
    internal class EnumFieldReader : IEnumFieldReader
    {
        /// <summary>
        ///     Фабрика кешей
        /// </summary>
        [NotNull] private readonly ICacherFactory _cacherFactory;

        /// <summary>
        ///     Инициализирует новый экземпляр класса <see cref="EnumFieldReader" />.
        /// </summary>
        /// <param name="cacherFactory">Фабрика кешей</param>
        public EnumFieldReader([NotNull] ICacherFactory cacherFactory)
        {
            if (cacherFactory == null) throw new ArgumentNullException(nameof(cacherFactory));
            _cacherFactory = cacherFactory;
        }

        /// <summary>
        ///     Прочитать поле по значению енума
        /// </summary>
        /// <typeparam name="TEnum">Тип енума</typeparam>
        /// <param name="e">Значение енума</param>
        /// <returns>Поле</returns>
        public FieldInfo Field<TEnum>(TEnum e) where TEnum : struct
        {
            var cache = Cacher();
            var enumName = e.ToString();
            return cache.GetOrAdd($"{e.GetType().FullName}|{enumName}", s => e.GetType().GetField(enumName)).ThrowIfNull("field info");
        }

        /// <summary>
        ///     Прочитать поле по забоксенному значению енума
        /// </summary>
        /// <param name="e">Значение енума</param>
        /// <returns>Поле</returns>
        public FieldInfo Field(Enum e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            var cache = Cacher();
            var enumName = e.ToString();
            return cache.GetOrAdd($"{e.GetType().FullName}|{enumName}", s => e.GetType().GetField(enumName)).ThrowIfNull("field info");
        }

        [NotNull]
        private ICacher<FieldInfo> Cacher()
        {
            return _cacherFactory.Create<FieldInfo>($"{typeof (EnumFieldReader).FullName}");
        }
    }
}