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

        public Expression ReplaceParameter(Expression e, ParameterExpression peFrom, ParameterExpression peTo)
        {
            var visiter = new ParameterRebinderVisiter(new Dictionary<ParameterExpression, ParameterExpression>
            {
                {peFrom, peTo}
            });
            return visiter.Visit(e);
        }

        
    }
}