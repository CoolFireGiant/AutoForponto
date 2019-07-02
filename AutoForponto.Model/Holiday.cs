using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AutoForponto.Model
{
    public static class Holiday
    {
        private static IList<DateTime> _currentYearHolidays;
        private static IList<DateTime> CurrentYearHolidays
        {
            get
            {
                if (_currentYearHolidays == null || _currentYearHolidays[0].Year < DateTime.Today.Year)
                {
                    _currentYearHolidays = GetHolidaysByCurrentYear();
                }
                return _currentYearHolidays;
            }
        }

        public static bool TodayIsWeekend
        {
            get
            {
                return
                    DateTime.Today.DayOfWeek == DayOfWeek.Saturday ||
                    DateTime.Today.DayOfWeek == DayOfWeek.Sunday;
            }
        }

        public static bool TodayIsHoliday
        {
            get
            {
                return CurrentYearHolidays.Contains(DateTime.Today);
            }
        }

        public static bool TodayIsWorkday
        {
            get
            {
                return !TodayIsWeekend && !TodayIsHoliday;
            }
        }

        private static readonly IDictionary<int, int[]> goldenNumberList = new Dictionary<int, int[]>() 
        {// { golden number, { month, day } }
            { 00, new int[] { 04, 14 } }, 
            { 01, new int[] { 04, 03 } }, 
            { 02, new int[] { 03, 23 } }, 
            { 03, new int[] { 04, 11 } }, 
            { 04, new int[] { 03, 31 } }, 
            { 05, new int[] { 04, 18 } }, 
            { 06, new int[] { 04, 08 } }, 
            { 07, new int[] { 03, 28 } }, 
            { 08, new int[] { 04, 16 } }, 
            { 09, new int[] { 04, 05 } }, 
            { 10, new int[] { 03, 25 } }, 
            { 11, new int[] { 04, 13 } },
            { 12, new int[] { 04, 02 } }, 
            { 13, new int[] { 03, 22 } }, 
            { 14, new int[] { 04, 10 } }, 
            { 15, new int[] { 03, 30 } }, 
            { 16, new int[] { 04, 17 } }, 
            { 17, new int[] { 04, 07 } }, 
            { 18, new int[] { 03, 27 } }
        };

        private static IList<DateTime> GetHolidaysByCurrentYear()
        {
            var holidayList = new List<DateTime>();
            var year = DateTime.Now.Year;

            int goldenNumber = year % 19;

            int month = goldenNumberList[goldenNumber][0];
            int day = goldenNumberList[goldenNumber][1];

            DateTime goldenDate = new DateTime(year, month, day);

            var pascoa = goldenDate.AddDays(7 - (int)goldenDate.DayOfWeek);
            var sextaSanta = pascoa.AddDays(-2);
            var carnaval = pascoa.AddDays(-47);
            var corpusChristi = pascoa.AddDays(60);

            #region Feriados móveis

            holidayList.Add(pascoa);
            holidayList.Add(sextaSanta);
            holidayList.Add(carnaval);
            holidayList.Add(carnaval.AddDays(-1)); //segunda-feira de carnaval
            holidayList.Add(corpusChristi);

            #endregion
            #region Feriados nacionais fixos

            holidayList.Add(new DateTime(year, 01, 01)); //Ano novo 
            holidayList.Add(new DateTime(year, 04, 21)); //Tiradentes
            holidayList.Add(new DateTime(year, 05, 01)); //Dia do trabalho
            holidayList.Add(new DateTime(year, 09, 07)); //Dia da Independência do Brasil
            holidayList.Add(new DateTime(year, 10, 12)); //Nossa Senhora Aparecida
            holidayList.Add(new DateTime(year, 11, 02)); //Finados
            holidayList.Add(new DateTime(year, 11, 15)); //Proclamação da República
            holidayList.Add(new DateTime(year, 12, 25)); //Natal

            #endregion
            #region Feriados estaduais (RJ) fixos

            holidayList.Add(new DateTime(year, 04, 23)); //São Jorge
            holidayList.Add(new DateTime(year, 11, 20)); //Dia da Consciência Negra

            #endregion
            #region Feriados municipais fixos (Rio de Janeiro)

            holidayList.Add(new DateTime(year, 01, 20)); //São Sebastião
            holidayList.Add(new DateTime(year, 03, 01)); //Aniversário da Cidade do Rio de Janeiro

            #endregion

            return holidayList;
        }
    }
}
