namespace Rib.Common.Helpers.Expressions
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof (ParameterMapFactory))]
    public interface IParameterMapFactory
    {
        [NotNull]
        IDictionary<ParameterExpression, ParameterExpression> Create([NotNull] LambdaExpression source, [NotNull] IEnumerable<LambdaExpression> expressions);
    }
}