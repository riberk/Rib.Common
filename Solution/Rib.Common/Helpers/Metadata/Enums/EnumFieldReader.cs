namespace Rib.Common.Helpers.Metadata.Enums
{
    using System;
    using System.Reflection;
    using Rib.Common.Helpers.Cache;
    using JetBrains.Annotations;

    /// <summary>
    ///     ���������� �������� ����� �� ��������� �����
    /// </summary>
    internal class EnumFieldReader : IEnumFieldReader
    {
        /// <summary>
        ///     ������� �����
        /// </summary>
        [NotNull] private readonly ICacherFactory _cacherFactory;

        /// <summary>
        ///     �������������� ����� ��������� ������ <see cref="EnumFieldReader" />.
        /// </summary>
        /// <param name="cacherFactory">������� �����</param>
        public EnumFieldReader([NotNull] ICacherFactory cacherFactory)
        {
            if (cacherFactory == null) throw new ArgumentNullException(nameof(cacherFactory));
            _cacherFactory = cacherFactory;
        }

        /// <summary>
        ///     ��������� ���� �� �������� �����
        /// </summary>
        /// <typeparam name="TEnum">��� �����</typeparam>
        /// <param name="e">�������� �����</param>
        /// <returns>����</returns>
        public FieldInfo Field<TEnum>(TEnum e) where TEnum : struct
        {
            var cache = Cacher();
            var enumName = e.ToString();
            return cache.GetOrAdd($"{e.GetType().FullName}|{enumName}", s => e.GetType().GetField(enumName)).ThrowIfNull("field info");
        }

        /// <summary>
        ///     ��������� ���� �� ������������ �������� �����
        /// </summary>
        /// <param name="e">�������� �����</param>
        /// <returns>����</returns>
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