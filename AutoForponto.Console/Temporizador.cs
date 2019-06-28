using System;
using System.Collections.Generic;
using System.Timers;
using AutoForponto.Model;

namespace AutoForponto.Console
{
    public static class Temporizador
    {
        private static string _login;
        private static string _senha;

        static double oneDay = 24 * 60 * 60 * 1000;
        static Random randomDelay = new Random();
        static List<Timer> timers = new List<Timer>();
        static List<DateTime> times = new List<DateTime> 
        {
            DateTime.Today.AddHours(9),     //Inicio da jornada
            DateTime.Today.AddHours(12),    //Inicio do almoço
            DateTime.Today.AddHours(13),    //Fim do almoço
            DateTime.Today.AddHours(18)     //Fim da jornada
        };

        public static void Start(string login, string senha)
        {
            _login = login;
            _senha = senha;

            foreach (var t in times)
            {
                var time = t.AddMinutes(-15);
                if (time < DateTime.Now)
                    time.AddDays(1);

                var interval = time.Subtract(DateTime.Now).TotalSeconds * 1000;
                if (interval <= 0) interval += oneDay;

                var timer = new Timer();
                timer.Enabled = true;
                timer.Interval = interval;
                timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);

                timers.Add(timer);
            }
        }

        private static void Timer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            var timer = (Timer)sender;

            if (Holiday.TodayIsWorkday)
            {
                System.Threading.Thread.Sleep(randomDelay.Next(30 * 60 * 1000));
                Marcador.MarcarPonto(_login, _senha);
            }

            if (timer.Interval != oneDay) timer.Interval = oneDay;
        }
    }
}
