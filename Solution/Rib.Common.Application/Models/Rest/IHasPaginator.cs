namespace Rib.Common.Application.Models.Rest
{
    using Rib.Common.Models.Contract;

    public interface IHasPaginator
    {
        Paginator Pagination { get; }
    }
}