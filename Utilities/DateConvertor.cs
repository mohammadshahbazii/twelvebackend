using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Utilities
{
	public static class DateConvertor
	{
        public static string GetPersianMonth(int number)
        {

            string month = "";
            switch (number)
            {
                case 1:
                    month = "فروردین";
                    break;
                case 2:
                    month =  "اردیبهشت";
                    break;
                case 3:
                    month =  "خرداد";
                    break;
                case 4:
                    month =  "تیر";
                    break;
                case 5:
                    month =  "مرداد";
                    break;
                case 6:
                    month =  "شهریور";
                    break;
                case 7:
                    month =  "مهر";
                    break;
                case 8:
                    month =  "آبان";
                    break;
                case 9:
                    month =  "آذر";
                    break;
                case 10:
                    month =  "دی";
                    break;
                case 11:
                    month =  "بهمن";
                    break;
                case 12:
                    month =  "اسفند";
                    break;
            }
            return month;
        }

		public static string GetPersianDate(DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();

            string DayName ="";
            if (date.DayOfWeek == DayOfWeek.Friday)
                DayName = "جمعه";
            else if (date.DayOfWeek == DayOfWeek.Saturday)
                DayName = "شنبه";
            else if (date.DayOfWeek == DayOfWeek.Sunday)
                DayName = "یکشنبه";
            else if (date.DayOfWeek == DayOfWeek.Monday)
                DayName = "دوشنبه";
            else if (date.DayOfWeek == DayOfWeek.Tuesday)
                DayName = "سه شنبه";
            else if (date.DayOfWeek == DayOfWeek.Wednesday)
                DayName = "چهار شنبه";
            else if (date.DayOfWeek == DayOfWeek.Thursday)
                DayName = "پنج شنبه";

            string result = DayName + "، "+ pc.GetDayOfMonth(date).ToString("00") + " ";
			switch (pc.GetMonth(date))
			{
				case 1:
					result = result + "فروردین ماه";
					break;
                case 2:
                    result = result + "اردیبهشت ماه";
                    break;
                case 3:
                    result = result + "خرداد ماه";
                    break;
                case 4:
                    result = result + "تیر ماه";
                    break;
                case 5:
                    result = result + "مرداد ماه";
                    break;
                case 6:
                    result = result + "شهریور ماه";
                    break;
                case 7:
                    result = result + "مهر ماه";
                    break;
                case 8:
                    result = result + "آبان ماه";
                    break;
                case 9:
                    result = result + "آذر ماه";
                    break;
                case 10:
                    result = result + "دی ماه";
                    break;
                case 11:
                    result = result + "بهمن ماه";
                    break;
                case 12:
                    result = result + "اسفند ماه";
                    break;
            }
            result += pc.GetYear(date) + " " ;
            return result;
        }

        public static string PassDays(DateTime dateTime)
        {
            DateTime today = DateTime.Now.Date;
            int passYears = today.Year - dateTime.Year;
			int passDays = (today.DayOfYear - dateTime.DayOfYear) + (passYears * 365);
			if (passDays == 0)
			{
				return "امروز";
			}
			return $"{passDays} روز قبل";
		}

		public static string ToShamsi(DateTime date)
		{
			PersianCalendar pc = new PersianCalendar();
			return pc.GetYear(date) + "/" + pc.GetMonth(date).ToString("00") + "/" + pc.GetDayOfMonth(date).ToString("00");
		}
        public static DateTime ToShamsiDate(DateTime date)
        {
            PersianCalendar pc = new PersianCalendar();
			DateTime dateTime =  new DateTime(pc.GetYear(date), pc.GetMonth(date), pc.GetDayOfMonth(date));

            return dateTime.Date;
        }

        public static string ToShamsi(DateTime? date)
        {
			if (date == null)
			{
				return "";
			}
			else
			{
				DateTime time = Convert.ToDateTime(date);
				return ToShamsi(time);
            }
        }

		public static DateTime ToMiladi(DateTime date)
		{
			return new DateTime(date.Year,date.Month,date.Day, new PersianCalendar());
		}
        public static DateTime ToMiladi(string date)
        {
			DateTime dateTime = Convert.ToDateTime(date);
            return new DateTime(dateTime.Year, dateTime.Month, dateTime.Day, new PersianCalendar());
        }
    }
}
