namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Linq.Expressions;
    using JetBrains.Annotations;

    public static class Clousure
    {
        [NotNull]
        public static Expression ToClousure<T>(T val)
        {
            Expression<Func<T>> expr = () => val;
            return expr.Body;
        }
    }
}