namespace Rib.Common.Application.Web.WebApi.Helpers
{
    using System;
    using System.Runtime.Remoting.Messaging;
    using JetBrains.Annotations;
    using Rib.Common.Application.Web.Owin;
    using Rib.Common.Helpers.CorrelationId;
    using Rib.Common.Models.Helpers;
    using Rib.Common.Models.Metadata;

    [BindFrom(typeof(ICorrelationIdStore))]
    public class CorrelationIdStore : ICorrelationIdStore
    {
        private const string CorrelationId = "correlation-id";
        [NotNull] private readonly IOwinContextResolver _owinContextResolver;

        public CorrelationIdStore([NotNull] IOwinContextResolver owinContextResolver)
        {
            if (owinContextResolver == null) throw new ArgumentNullException(nameof(owinContextResolver));
            _owinContextResolver = owinContextResolver;
        }


        public CorrelationId Read()
        {
            return _owinContextResolver.Current?.Get<CorrelationId>(CorrelationId) ?? CallContext.LogicalGetData(CorrelationId) as CorrelationId;
        }

        public void Save(CorrelationId id)
        {
             _owinContextResolver.Current?.Set(CorrelationId, id);
            CallContext.LogicalSetData(CorrelationId, id);
        }
    }
}