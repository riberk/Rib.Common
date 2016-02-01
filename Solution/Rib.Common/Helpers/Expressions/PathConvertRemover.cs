namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Linq.Expressions;
    using JetBrains.Annotations;

    public class PathConvertRemover : IPathConvertRemover
    {
        public LambdaExpression RemoveLast(LambdaExpression e)
        {
            if (e == null) throw new ArgumentNullException(nameof(e));
            if (e.Parameters.Count != 1)
            {
                throw new ArgumentOutOfRangeException(nameof(e.Parameters), "Число параметров должно быть равно единице");
            }
            var param = e.Parameters[0];
            var me = RemoveLastOnPath(e.Body);
            return Expression.Lambda(me, param);
        }

        [NotNull]
        private MemberExpression RemoveLastOnPath([NotNull] Expression e)
        {
            MemberExpression me;
            var unary = e as UnaryExpression;
            if (unary != null)
            {
                me = unary.Operand as MemberExpression;
            }
            else
            {
                me = e as MemberExpression;
            }
            if (me == null)
            {
                throw new InvalidCastException($"Can not cast {e.GetType()} to {typeof (MemberExpression)}");
            }
            return me;
        }
    }
}