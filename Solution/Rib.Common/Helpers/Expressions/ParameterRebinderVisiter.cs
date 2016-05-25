namespace Rib.Common.Helpers.Expressions
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using JetBrains.Annotations;

    public class ParameterRebinderVisiter : ExpressionVisitor
    {
        [NotNull]
        private readonly IDictionary<ParameterExpression, ParameterExpression> _map;

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