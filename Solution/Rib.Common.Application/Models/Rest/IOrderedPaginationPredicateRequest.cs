namespace Rib.Common.Application.Models.Rest
{
    public interface IOrderedPaginationPredicateRequest<T> : IOrderedPaginationRequest, IPredicateProvider<T>
    {
    }
}