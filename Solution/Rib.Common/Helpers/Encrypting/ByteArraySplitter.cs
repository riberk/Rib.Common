namespace Rib.Common.Helpers.Encrypting
{
    using System;
    using System.Linq;

    internal class ByteArraySplitter : IByteArraySplitter
    {
        /// <summary>
        ///     Разбить исходный массив на блоки фиксированной длинны
        /// </summary>
        /// <param name="array">Исзодный массив</param>
        /// <param name="blockLength">Размер блока</param>
        /// <returns>Двумерный массив</returns>
        public byte[][] Split(byte[] array, int blockLength)
        {
            if (array == null) throw new ArgumentNullException(nameof(array));
            if (blockLength <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(blockLength), "Размер блока не может быть меньше либо равным нулю");
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