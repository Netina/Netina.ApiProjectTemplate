using MD.PersianDateTime.Standard;

namespace Common.Extensions;

public static class DateTimeExtensions
{
    public static string GetPersianDayOfWeek(this DayOfWeek dayOfWeek)
    {
        switch (dayOfWeek)
        {
            case DayOfWeek.Friday:
                return "جمعه";
            case DayOfWeek.Monday:
                return "دوشنبه";
            case DayOfWeek.Saturday:
                return "شنبه";
            case DayOfWeek.Sunday:
                return "یکشنبه";
            case DayOfWeek.Thursday:
                return "پنج شنبه";
            case DayOfWeek.Tuesday:
                return "سه شنبه";
            case DayOfWeek.Wednesday:
                return "چهارشنبه";
        }

        return "";
    }

    public static DateTime UnixTimeStampToDateTime(double unixTimeStamp)
    {
        // Unix timestamp is seconds past epoch
        var dtDateTime = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc);
        dtDateTime = dtDateTime.AddMilliseconds(unixTimeStamp).ToLocalTime();
        return dtDateTime;
    }

    public static long DateTimeToUnixTimeStamp(DateTime dateTime)
    {
        return ((DateTimeOffset)dateTime).ToUnixTimeMilliseconds();
    }

    public static int DifferenceByDay(DateTime originDateTime, DateTime destDateTime)
    {
        return (int)(destDateTime - originDateTime).TotalDays;
    }

    public static int DifferenceByHoure(DateTime originDateTime, DateTime destDateTime)
    {
        return (int)(destDateTime - originDateTime).TotalHours;
    }

    public static TimeSpan Difference(DateTime originDateTime, DateTime destDateTime)
    {
        var durateion = (destDateTime - originDateTime).Duration();
        return durateion;
    }
    public static PersianDateTime ToPersianDateTime(this DateTime dateTime)
       => new PersianDateTime(dateTime);
}