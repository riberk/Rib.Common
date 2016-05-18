namespace Rib.Common.Application.CorrelationId
{
    using Rib.Common.Models.Helpers;
    using TsSoft.ContextWrapper;

    public class CorrelationIdFactory : IItemFactory<CorrelationId>
    {
        public CorrelationId Create(string key)
        {
            return new CorrelationId();
        }
    }
}