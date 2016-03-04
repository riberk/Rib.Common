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
    }
}