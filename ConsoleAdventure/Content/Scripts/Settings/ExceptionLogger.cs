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
            if (IsFileLocked(logFilePath)) return;

            TryLogExist();
            File.AppendAllText(logFilePath, text);
            Console.WriteLine(text);
        }

        public void AddMessage(string message)
        {
            AddText($"{DateTime.Now}: {message}\n");
        }

        public void AddException(string error)
        {
            string exceptionMessage = $"{DateTime.Now}: [EXCEPTION] {error}";
            AddText(exceptionMessage + Environment.NewLine);
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

        static bool IsFileLocked(string filePath)
        {
            try
            {
                using (FileStream fs = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite, FileShare.None))
                {
                }
            }
            catch (IOException)
            {
                // Исключение указывает, что файл используется
                return true;
            }
            return false;
        }
    }
}
