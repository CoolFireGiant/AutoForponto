using System;
using System.IO;
using System.Linq;

namespace AutoForponto.Model
{
    public class Logger
    {
        public string Login { get; set; }
        public string FilePath 
        {
            get { return string.Format(@"AutoForponto Log - {0}.csv", DateTime.Today.ToString("yyyyMM")); } 
        }

        public Logger(string login = "SERVICE")
        {
            Login = login;
            LogStartOfDay();
        }

        public void Log(string text)
        {
            File.AppendAllText(FilePath, Environment.NewLine + string.Join(";", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff"), Login, text));
        }

        private void LogStartOfDay()
        {
            string line = string.Empty;

            if (File.Exists(FilePath))
            {
                var lines = File.ReadLines(FilePath);
                if (lines.Count() > 0)
                {
                    line = lines.Last();
                }
            }

            if (string.IsNullOrWhiteSpace(line) || (
                line[0] != '=' &&
                DateTime.Parse(line.Split(' ')[0]) != DateTime.Today))
            {
                File.AppendAllText(FilePath, string.Format("{0}========== {1} ==========", Environment.NewLine, DateTime.Today.ToLongDateString()));
            }
        }
    }
}
