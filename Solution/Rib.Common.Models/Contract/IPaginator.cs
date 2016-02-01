namespace Rib.Common.Models.Contract
{
    public interface IPaginator
    {
        int PageNumber { get; }
        int PageSize { get; }
        int Skip { get; }
    }
}