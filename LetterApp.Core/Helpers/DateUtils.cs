using System;
using LetterApp.Core.Localization;

namespace LetterApp.Core.Helpers
{
    public static class DateUtils
    {
        const int SECOND = 1;
        const int MINUTE = 60 * SECOND;
        const int HOUR = 60 * MINUTE;
        const int DAY = 24 * HOUR;
        const int MONTH = 30 * DAY;

        public static bool CompareDates(long date1, DateTime date2)
        {
            return new DateTime(date1).Date == date2.Date;
        }

        //https://stackoverflow.com/questions/11/calculate-relative-time-in-c-sharp
        public static string TimePassed(DateTime date)
        {
            var ts = new TimeSpan(DateTime.Now.Ticks - date.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (delta < 1 * MINUTE)
                return ts.Seconds + L10N.Localize("date_Seconds");

            if (delta < 2 * MINUTE)
                return L10N.Localize("date_Minute");

            if (delta < 45 * MINUTE)
                return ts.Minutes + L10N.Localize("date_Minutes");

            if (delta < 90 * MINUTE)
                return L10N.Localize("date_Hour");

            if (delta <= 9 * HOUR)
                return ts.Hours + L10N.Localize("date_Hours");

            if (delta > 9 * HOUR && date.Date == DateTime.Now.Date)
                return L10N.Localize("date_Today");

            if (date.Date.AddDays(1) == DateTime.Now.Date)
                return L10N.Localize("date_Yesterday");

            if (delta > 48 * HOUR && delta < 144 * HOUR || date.Date.AddDays(2) == DateTime.Now.Date)
                return L10N.Localize($"date_{date.DayOfWeek.ToString()}");
                
            return date.Date.ToShortDateString();
        }

        public static string TimeForChat(DateTime date)
        {
            var ts = new TimeSpan(DateTime.Now.Ticks - date.Ticks);
            double delta = Math.Abs(ts.TotalSeconds);

            if (date.Year != DateTime.Now.Year)
                return date.Date.ToShortDateString();

            if (date.Date == DateTime.Now.Date || delta <= 18 * HOUR)
                return date.ToString("HH:mm");

            //if(date.Date.AddDays(1) == DateTime.Now.Date)
                //return L10N.Localize("date_Yesterday");

            if (delta > 48 * HOUR && delta < 144 * HOUR || date.Date.AddDays(1) == DateTime.Now.Date)
                return L10N.Localize($"date_{date.DayOfWeek.ToString()}");

            string month = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetAbbreviatedMonthName(date.Month);

            return L10N.Locale() == "en-US" ? $"{month} {date.Day}" : $"{date.Day} {month}";
        }
    }
}
