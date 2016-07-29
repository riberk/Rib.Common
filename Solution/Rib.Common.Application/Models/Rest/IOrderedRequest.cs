namespace Rib.Common.Application.Models.Rest
{
    using System.Collections.Generic;

    public interface IOrderedRequest
    {
        IReadOnlyCollection<KeyValuePair<string, bool>> Order { get; }
    }
}