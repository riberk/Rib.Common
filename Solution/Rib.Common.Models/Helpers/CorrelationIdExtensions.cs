namespace Rib.Common.Models.Helpers
{
    using System;

    public static class CorrelationIdExtensions
    {
        public static string ToDatedString(this CorrelationId cid)
        {
            Guid id = cid;
            return $"{id}__{DateTime.UtcNow.ToString("yyyyMMdd_hhmmss")}";
        }
    }
}