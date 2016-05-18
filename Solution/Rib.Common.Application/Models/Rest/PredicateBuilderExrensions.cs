namespace TsSoft.Expressions.Helpers
{
    using System;
    using System.Linq.Expressions;
    using JetBrains.Annotations;

    public static class PredicateBuilderExrensions
    {
        [NotNull]
        public static IPredicateBuilder<T> And<T>([NotNull] this IPredicateBuilder<T> pb, Expression<Func<T, bool>> expr, bool needAnd)
        {
            if (pb == null) throw new ArgumentNullException(nameof(pb));
            return needAnd ? pb.And(expr) : pb;
        }

        [NotNull]
        public static IPredicateBuilder<T> Or<T>([NotNull] this IPredicateBuilder<T> pb, Expression<Func<T, bool>> expr, bool needOr)
        {
            if (pb == null) throw new ArgumentNullException(nameof(pb));
            return needOr ? pb.Or(expr) : pb;
        }
    }
}