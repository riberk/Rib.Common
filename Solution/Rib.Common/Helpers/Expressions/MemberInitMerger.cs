namespace Rib.Common.Helpers.Expressions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using JetBrains.Annotations;
    using Rib.Common.Models.Exceptions;

    internal class MemberInitMerger : IMemberInitMerger
    {
        [NotNull] private readonly IParameterRebinder _parameterRebinder;

        public MemberInitMerger([NotNull] IParameterRebinder parameterRebinder)
        {
            if (parameterRebinder == null) throw new ArgumentNullException(nameof(parameterRebinder));
            _parameterRebinder = parameterRebinder;
        }

        public Expression Merge<T>(IEnumerable<LambdaExpression> expressions, out ParameterExpression pe)
        {
            if (expressions == null) throw new ArgumentNullException(nameof(expressions));
            var constructorInfo = typeof(T).GetConstructor(new Type[0]);
            if (constructorInfo == null)
            {
                throw new MetadataException($"Parameterless constructor on {typeof(T)} could not be found");
            }
            var lambdaExpressions = expressions as LambdaExpression[] ?? expressions.ToArray();
            var rebinded = _parameterRebinder.ReplaceParametersByFirst(lambdaExpressions);
            var newExpression = Expression.New(constructorInfo);
            var bindings = Enumerable.Empty<MemberBinding>();
            foreach (var mie in rebinded.Select(expression => expression as MemberInitExpression))
            {
                if (mie == null)
                {
                    throw new RibCommonException("Expression is not member init");
                }
                bindings = bindings.Concat(mie.Bindings);
            }
            var mInit = Expression.MemberInit(newExpression, bindings);
            pe = lambdaExpressions[0].Parameters[0];
            return mInit;
        }
    }
}