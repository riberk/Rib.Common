namespace Rib.Common.Helpers.DateAndTime
{
    using System;
    using JetBrains.Annotations;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(DateTimeLocalizer))]
    public interface IDateTimeLocalizer
    {
        DateTime ToLocal(DateTime dt, [NotNull] string tzi);

        DateTime ToUtc(DateTime dt, [NotNull] string tzi);
    }
}