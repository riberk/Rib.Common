namespace Rib.Common.Application.ClientEnums
{
    using System;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(ClientEnumInitializer))]
    public interface IClientEnumInitializer
    {
        void Initialize(params Type[] types);
    }
}