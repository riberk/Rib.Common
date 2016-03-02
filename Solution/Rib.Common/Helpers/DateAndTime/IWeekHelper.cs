namespace Rib.Common.Helpers.DateAndTime
{
    using System;
    using Rib.Common.Models.Metadata;

    [BindTo(typeof(WeekHelper))]
    public interface IWeekHelper
    {
        DateTime GetFirstDayOfWeek(DateTime dt);

        DateTime GetFirstDayOfWeek(int year, int week);
    }
}