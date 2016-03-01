namespace Rib.Common.Helpers.Cache
{
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    /// <summary>
    ///     ���������� ����
    /// </summary>
    [BindTo(typeof(CacheCleaner))]
    public interface ICacheCleaner
    {
        /// <summary>
        ///     �������� ��� ����
        /// </summary>
        void Clean();

        /// <summary>
        ///     �������� ���� ���� �� �����
        /// </summary>
        /// <param name="fullName">������ ���� �����</param>
        void Clean([NotNull] string fullName);
    }
}