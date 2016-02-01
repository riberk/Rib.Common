namespace Rib.Common.Helpers.CorrelationId
{
    using Rib.Common.Models.Helpers;

    public interface ICorrelationIdStore
    {
        CorrelationId Read();

        void Save(CorrelationId id);
    }
}