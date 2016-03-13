namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;

    internal class ParameterMapFactory : IParameterMapFactory
    {
        public IDictionary<ParameterExpression, ParameterExpression> Create(LambdaExpression source, IEnumerable<LambdaExpression> expressions)
        {
            if (source == null) throw new ArgumentNullException(nameof(source));
            if (expressions == null) throw new ArgumentNullException(nameof(expressions));
            var selectMany = expressions.SelectMany(e =>
                                                    e.Parameters.Select((p, i) => new {sourceParam = source.Parameters[i], destParam = p}));
            return selectMany
                              .ToDictionary(r => r.destParam, r => r.sourceParam);
        }
    }
}