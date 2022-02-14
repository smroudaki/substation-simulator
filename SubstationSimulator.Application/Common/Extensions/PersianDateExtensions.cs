using System;
using System.Collections.Generic;
using System.Globalization;
using System.Reflection;
using System.Text;

namespace ElectricalEmulator.Application.Common.Extensions
{
    public static class PersianDateExtensions
    {
        private static CultureInfo _culture;

        public static CultureInfo GetPersianCulture()
        {
            if (_culture == null)
            {
                _culture = new CultureInfo("fa-IR");

                var monthNames = new[] { "فروردین", "اردیبهشت", "خرداد", "تیر", "مرداد", "شهریور", "مهر", "آبان", "آذر", "دی", "بهمن", "اسفند", "" };

                var formatInfo = _culture.DateTimeFormat;
                formatInfo.AbbreviatedDayNames = new[] { "ی", "د", "س", "چ", "پ", "ج", "ش" };
                formatInfo.DayNames = new[] { "یکشنبه", "دوشنبه", "سه شنبه", "چهار شنبه", "پنجشنبه", "جمعه", "شنبه" };
                formatInfo.AbbreviatedMonthNames = formatInfo.MonthNames = formatInfo.MonthGenitiveNames = formatInfo.AbbreviatedMonthGenitiveNames = monthNames;
                formatInfo.AMDesignator = "ق.ظ";
                formatInfo.PMDesignator = "ب.ظ";
                formatInfo.ShortDatePattern = "yyyy/MM/dd";
                formatInfo.LongDatePattern = "dddd, dd MMMM,yyyy";
                formatInfo.FirstDayOfWeek = DayOfWeek.Saturday;

                var cal = new PersianCalendar();

                var fieldInfo = _culture.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);

                if (fieldInfo != null)
                {
                    fieldInfo.SetValue(_culture, cal);
                }

                var info = formatInfo.GetType().GetField("calendar", BindingFlags.NonPublic | BindingFlags.Instance);

                if (info != null)
                {
                    info.SetValue(formatInfo, cal);
                }

                _culture.NumberFormat.NumberDecimalSeparator = "/";
                _culture.NumberFormat.DigitSubstitution = DigitShapes.NativeNational;
                _culture.NumberFormat.NumberNegativePattern = 0;
            }

            return _culture;
        }

        public static string ToPeString(this DateTime date, string format = "yyyy/MM/dd")
        {
            return date.ToString(format, GetPersianCulture());
        }
    }
}
