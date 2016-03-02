namespace Rib.Common.Helpers.Collections
{
    using System;
    using System.Collections.Generic;
    using JetBrains.Annotations;

    public static class LinkedListNodeExtensions
    {
        [NotNull]
        public static LinkedListNode<T> NextOrFirst<T>([NotNull] this LinkedListNode<T> current)
        {
            if (current == null) throw new ArgumentNullException(nameof(current));
            return current.Next ?? current.List.First;
        }

        [NotNull]
        public static LinkedListNode<T> PreviousOrLast<T>([NotNull] this LinkedListNode<T> current)
        {
            if (current == null) throw new ArgumentNullException(nameof(current));
            return current.Previous ?? current.List.Last;
        }
    }
}