namespace Rib.Common.Helpers.Cache
{
    using JetBrains.Annotations;

    /// <summary>
    ///     ���������� ����
    /// </summary>
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