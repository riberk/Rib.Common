namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Linq.Expressions;
    using JetBrains.Annotations;

    public static class ExpressionExtensions
    {
        [NotNull]
        public static T RemoveConvert<T>([NotNull] this T sourceExpression)
            where T : Expression
        {
            if (sourceExpression == null) throw new ArgumentNullException(nameof(sourceExpression));
            return (T)new ConvertRemover().Visit(sourceExpression);
        }

        /// <summary>
        ///     Removes unnessesary Convert() operands (inserted by Roslyn, but not supported by EF).
        /// </summary>
        private sealed class ConvertRemover : ExpressionVisitor
        {
            protected override Expression VisitUnary(UnaryExpression node)
            {
                if (node.NodeType == ExpressionType.Convert && node.Type != typeof(object) && node.Type.IsAssignableFrom(node.Operand.Type))
                    return Visit(node.Operand);
                return base.VisitUnary(node);
            }
        }
    }
}