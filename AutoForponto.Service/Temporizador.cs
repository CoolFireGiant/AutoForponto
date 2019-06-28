using System;
using System.Collections.Generic;
using System.Timers;
using AutoForponto.Model;

namespace AutoForponto.Service
{
    public partial class Temporizador : System.ServiceProcess.ServiceBase
    {
        double oneDay = 24 * 60 * 60 * 1000;
        List<Timer> timers;
        List<DateTime> times;
        Random randomDelay;

        public Temporizador()
        {
            InitializeComponent();
            randomDelay = new Random();
            timers = new List<Timer>();
            times = new List<DateTime> 
            {
                DateTime.Today.AddHours(9),     //Inicio da jornada
                DateTime.Today.AddHours(12),    //Inicio do almoço
                DateTime.Today.AddHours(13),    //Fim do almoço
                DateTime.Today.AddHours(18)     //Fim da jornada
            };
        }

        protected override void OnStart(string[] args)
        {
            new Logger().Log("SERVICE STARTED");

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

        protected override void OnStop()
        {
            new Logger().Log("SERVICE STOPPED");
        }

        protected void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var timer = (Timer)sender;

            if (Holiday.TodayIsWorkday)
            {
                System.Threading.Thread.Sleep(randomDelay.Next(30 * 60 * 1000));
                Marcador.MarcarPonto(
                    Properties.Settings.Default.Login, 
                    Properties.Settings.Default.Senha);
            }

            if (timer.Interval != oneDay)
            {
                timer.Interval = oneDay;
            }
        }
    }
}
