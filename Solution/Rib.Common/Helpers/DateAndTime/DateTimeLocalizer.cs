namespace Rib.Common.Helpers.DateAndTime
{
    using System;

    internal class DateTimeLocalizer : IDateTimeLocalizer
    {
        public DateTime ToLocal(DateTime dt, string tzi)
        {
            if (tzi == null) throw new ArgumentNullException(nameof(tzi));
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(tzi);
            if (dt.Kind == DateTimeKind.Local && timeZone.Equals(TimeZoneInfo.Local))
            {
                return dt;
            }
            if (dt.Kind == DateTimeKind.Local)
            {
                throw new ArgumentException("≈сли дата имеет Kind == Local, то временна€ зона может быть только TimeZoneInfo.Local", nameof(dt));
            }
            return TimeZoneInfo.ConvertTimeFromUtc(dt, timeZone);
        }

        public DateTime ToUtc(DateTime dt, string tzi)
        {
            if (tzi == null) throw new ArgumentNullException(nameof(tzi));
            if (dt.Kind == DateTimeKind.Utc)
            {
                return dt;
            }
            var timeZone = TimeZoneInfo.FindSystemTimeZoneById(tzi);
            if (dt.Kind == DateTimeKind.Local)
            {
                dt = new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, dt.Minute, dt.Second, dt.Millisecond, DateTimeKind.Unspecified);
            }
            return TimeZoneInfo.ConvertTimeToUtc(dt, timeZone);
        }
    }
}