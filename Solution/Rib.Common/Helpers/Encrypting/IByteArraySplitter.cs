namespace Rib.Common.Helpers.Encrypting
{
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(ByteArraySplitter))]
    public interface IByteArraySplitter
    {
        /// <summary>
        ///     Разбить исходный массив на блоки фиксированной длинны
        /// </summary>
        /// <param name="array">Исзодный массив</param>
        /// <param name="blockLength">Размер блока</param>
        /// <returns>Двумерный массив</returns>
        [NotNull, ItemNotNull]
        byte[][] Split([NotNull] byte[] array, int blockLength);
    }
}