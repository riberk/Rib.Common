namespace Rib.Common.Helpers.Encrypting
{
    using JetBrains.Annotations;

    public interface IByteArraySplitter
    {
        /// <summary>
        ///     ������� �������� ������ �� ����� ������������� ������
        /// </summary>
        /// <param name="array">�������� ������</param>
        /// <param name="blockLength">������ �����</param>
        /// <returns>��������� ������</returns>
        [NotNull, ItemNotNull]
        byte[][] Split([NotNull] byte[] array, int blockLength);
    }
}