namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Linq.Expressions;
    using JetBrains.Annotations;

    internal class ParameterRebinder : IParameterRebinder
    {
        [NotNull] private readonly IParameterMapFactory _mapFactory;

        public ParameterRebinder([NotNull] IParameterMapFactory mapFactory)
        {
            if (mapFactory == null) throw new ArgumentNullException(nameof(mapFactory));
            _mapFactory = mapFactory;
        }


        public IEnumerable<Expression> ReplaceParametersByFirst(params LambdaExpression[] expressions)
        {
            Debug.WriteLine(1);
            if (expressions == null) throw new ArgumentNullException(nameof(expressions));
            yield return expressions[0].Body;
            var map = _mapFactory.Create(expressions[0], expressions.Skip(1));
            var visitor = new ParameterRebinderVisiter(map);
            for (var i = 1; i < expressions.Length; i++)
            {
                yield return visitor.Visit(expressions[i].Body);
            }
        }

        internal class ParameterRebinderVisiter : ExpressionVisitor
        {
            [NotNull] private readonly IDictionary<ParameterExpression, ParameterExpression> _map;

            public ParameterRebinderVisiter(IDictionary<ParameterExpression, ParameterExpression> map)
            {
                _map = map ?? new Dictionary<ParameterExpression, ParameterExpression>();
            }

            protected override Expression VisitParameter(ParameterExpression p)
            {
                ParameterExpression parameterExpression;
                if (_map.TryGetValue(p, out parameterExpression) && parameterExpression != null)
                {
                    p = parameterExpression;
                }
                return base.VisitParameter(p);
            }
        }
    }
}