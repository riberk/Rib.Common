namespace Rib.Common.Helpers.Expressions
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof (ParameterRebinder))]
    public interface IParameterRebinder
    {
        [NotNull]
        IEnumerable<Expression> ReplaceParametersByFirst([NotNull] params LambdaExpression[] expressions);

        [NotNull]
        Expression ReplaceParameter([NotNull] Expression e, [NotNull] ParameterExpression peFrom, [NotNull] ParameterExpression peTo);
    }
}