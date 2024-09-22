using System;
using System.IO;

namespace ConsoleAdventure.Content.Scripts.Settings
{
    public class ExceptionLogger
    {
        private readonly string logsPath;
        private readonly string logFilePath;
        private string oldText = "";
        
        public ExceptionLogger(string logFileName)
        {
            logFilePath = Program.savePath + "Logs\\";
            logsPath = logFilePath;

            if (!Directory.Exists(logFilePath))
            {
                Directory.CreateDirectory(logFilePath);
            }

            logFilePath += logFileName;

            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath).Dispose();
            }

            AddText("------------------------------------------\n");
        }

        public void AddText(string text)
        {
            TryLogExist();
            File.AppendAllText(logFilePath, text);
            Console.WriteLine(text);
        }

        public void AddMassage(string massage)
        {
            AddText($"{DateTime.Now}: {massage}\n");
        }

        public void AddException(Exception ex)
        {
            string exceptionMessage = $"{DateTime.Now}: [EXCEPTION] {ex.GetType()}: {ex.Message}\n{ex.InnerException}\n{ex.StackTrace}\n{ex.Source}\n{ex.TargetSite}";
            AddText(exceptionMessage + Environment.NewLine);
        }

        private void TryLogExist()
        {
            if (!Directory.Exists(logsPath))
            {
                Directory.CreateDirectory(logsPath);
            }

            if (!File.Exists(logFilePath))
            {
                File.Create(logFilePath).Dispose();
            }
        }
    }
}
