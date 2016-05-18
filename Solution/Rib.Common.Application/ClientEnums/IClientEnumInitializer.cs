namespace Rib.Common.Application.ClientEnums
{
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(ClientEnumInitializer))]
    public interface IClientEnumInitializer
    {
        void Initialize(params object[] obj);
    }
}