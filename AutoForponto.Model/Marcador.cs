using System;
using System.Configuration;
using System.IO;
using OpenQA.Selenium.Chrome;

namespace AutoForponto.Model
{
    public static class Marcador
    {
        public static void MarcarPonto(string login, string senha)
        {
            var logger = new Logger(login);

            try
            {
                var appPath = System.Reflection.Assembly.GetExecutingAssembly().CodeBase;
                var driverpath = Path.GetDirectoryName(appPath).Remove(0, 6);

                var options = new ChromeOptions();
                options.AddArguments(new string[] { "--headless", "--disable-gpu", "--remote-debugging-port=9222" });

                ChromeDriver driver = new ChromeDriver(driverpath, options);

                try
                {
                    var limiteTentativas = 30;
                    var i = 0;
                    do
                    {
                        i++;
                        driver.Url = @"http://forponto.quality.com.br/forponto/FptoWeb.exe/PiEntraMarcacao";
                        System.Threading.Thread.Sleep(1000);
                        driver.Navigate().Refresh();
                        driver.FindElementByName("deEdtUsuCodigoAD").SendKeys(login);
                        driver.FindElementByName("deEdtUsaSenhaAD").SendKeys(senha);
                        driver.FindElementByName("ok").Click();
                    } while (!driver.Title.Contains("Marcação Aceita") && i < limiteTentativas);

                    if (driver.Title.Contains("Aviso"))
                        logger.Log(driver.FindElementByClassName("fonteavisos").Text);
                    else
                        logger.Log("Marcação Aceita");
                }
                catch (Exception e)
                {
                    while (e.InnerException != null) e = e.InnerException;
                    logger.Log(e.Message.Replace(Environment.NewLine, " "));
                }
                finally
                {
                    driver.Quit();
                }
            }
            catch (Exception e)
            {
                while (e.InnerException != null) e = e.InnerException;
                logger.Log(e.Message.Replace(Environment.NewLine, " "));
            }
        }
    }
}
