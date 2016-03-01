namespace Rib.Common.Helpers.Expressions
{
    using System.Linq.Expressions;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(PathConvertRemover))]
    public interface IPathConvertRemover
    {
        [NotNull]
        LambdaExpression RemoveLast([NotNull] LambdaExpression e);
    }
}