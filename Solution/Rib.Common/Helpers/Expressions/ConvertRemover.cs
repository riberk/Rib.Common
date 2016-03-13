namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Linq.Expressions;
    using JetBrains.Annotations;

    /// <summary>
    ///     Removes unnessesary Convert() operands (inserted by Roslyn, but not supported by EF).
    /// </summary>
    public class ConvertRemover : ExpressionVisitor
    {
        /// <summary>
        ///     Creates and simplificates the <paramref name="predicateExpression" />.
        ///     Can be used when building EnituyFramework LINQ.
        ///     <example>
        ///         var res = Db.Set{TEntity}().First(ConvertRemover.Remove{TEntity}(x => x.Id == id));
        ///     </example>
        /// </summary>
        /// <typeparam name="T">Remove source type.</typeparam>
        /// <param name="predicateExpression">Remove expression</param>
        [NotNull]
        public static Expression<Func<T, bool>> Remove<T>([NotNull] Expression<Func<T, bool>> predicateExpression)
        {
            return Remove<T, bool>(predicateExpression);
        }

        [NotNull]
        public static Expression<Func<T, TRes>> Remove<T, TRes>([NotNull] Expression<Func<T, TRes>> predicateExpression)
        {
            if (predicateExpression == null) throw new ArgumentNullException(nameof(predicateExpression));
            return (Expression<Func<T, TRes>>) new ConvertRemover().Visit(predicateExpression);
        }

        protected override Expression VisitUnary(UnaryExpression node)
        {
            // Replace explicit Convert to implicit one. (Except converion to Object).
            if (node.NodeType == ExpressionType.Convert && node.Type != typeof (object) && node.Type.IsAssignableFrom(node.Operand.Type))
                return Visit(node.Operand);
            return base.VisitUnary(node);
        }
    }
}