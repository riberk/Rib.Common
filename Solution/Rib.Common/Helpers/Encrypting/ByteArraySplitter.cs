namespace Rib.Common.Helpers.Encrypting
{
    using System;
    using System.Linq;

    internal class ByteArraySplitter : IByteArraySplitter
    {
        /// <summary>
        ///     ������� �������� ������ �� ����� ������������� ������
        /// </summary>
        /// <param name="array">�������� ������</param>
        /// <param name="blockLength">������ �����</param>
        /// <returns>��������� ������</returns>
        public byte[][] Split(byte[] array, int blockLength)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (blockLength <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(blockLength), "������ ����� �� ����� ���� ������ ���� ������ ����");
            }
            var blocksCount = (int) Math.Ceiling((double) array.Length/blockLength);
            var res = new byte[blocksCount][];
            for (var i = 0; i < blocksCount; i++)
            {
                var cur = array.Skip(i*blockLength).Take(blockLength).ToArray();
                res[i] = cur;
            }
            return res;
        }
    }
}