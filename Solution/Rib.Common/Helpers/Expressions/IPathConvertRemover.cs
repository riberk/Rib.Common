namespace Rib.Common.Helpers.Expressions
{
    using System.Linq.Expressions;
    using JetBrains.Annotations;

    public interface IPathConvertRemover
    {
        [NotNull]
        LambdaExpression RemoveLast([NotNull] LambdaExpression e);
    }
}