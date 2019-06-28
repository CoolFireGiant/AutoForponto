using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace AutoForponto.Model
{
    public class Logger
    {
        private string _login;

        private FileInfo LogFile
        {
            get
            {
                //var dirPath = string.Format(@"C:/Users/{0}/Documents/AutoForponto Log/", Environment.UserName);
                //if (!Directory.Exists(dirPath))
                //    Directory.CreateDirectory(dirPath);
                var dirPath = string.Empty;
                var filePath = string.Format(dirPath + @"AutoForponto Log - {0}.csv", DateTime.Today.ToString("yyyyMM"));
                return new FileInfo(filePath);
            }
        }

        public Logger()
        {
            _login = "SERVICE";
            LogStartOfDay();
        }

        public Logger(string login)
        {
            _login = login;
        }

        public void Log(string text)
        {
            using (var logWriter = LogFile.AppendText())
            {
                logWriter.AutoFlush = true;

                text = string.Join(";", DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss.fff"), _login, text);

                logWriter.Write(Environment.NewLine + text);
                logWriter.Close();
            }
        }

        private void LogStartOfDay()
        {
            string line = string.Empty;
            var vazio = true;

            if (LogFile.Exists)
            {
                using (var reader = LogFile.OpenText())
                {
                    while (!reader.EndOfStream)
                    {
                        line = reader.ReadLine();
                        vazio = false;
                    }
                    reader.Close();
                }
            }

            if (vazio || (
                line[0] != '=' &&
                !string.IsNullOrWhiteSpace(line) &&
                DateTime.Parse(line.Split(' ')[0]) < DateTime.Today))
            {
                using (var logWriter = LogFile.AppendText())
                {
                    logWriter.AutoFlush = true;
                    logWriter.Write((vazio ? "" : Environment.NewLine) + "========== " + DateTime.Today.ToLongDateString() + " ==========");
                    logWriter.Close();
                }
            }
        }
    }
}
