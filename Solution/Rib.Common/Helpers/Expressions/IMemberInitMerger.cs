namespace Rib.Common.Helpers.Expressions
{
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(MemberInitMerger))]
    public interface IMemberInitMerger
    {
        [NotNull]
        Expression Merge<T>([NotNull] IEnumerable<LambdaExpression> expressions, out ParameterExpression pe);
    }
}