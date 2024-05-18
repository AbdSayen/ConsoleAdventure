using Microsoft.VisualBasic.Logging;
using System.Collections.Generic;

namespace ConsoleAdventure.Settings
{
    static public class Loger
    {
        static List<string> logs = new List<string>();

        public static void AddLog(string log)
        {
            logs.Add(log);
        }

        public static void ClearLogs()
        {
            logs.Clear();
        }

        public static string GetLogs()
        {
            string output = string.Empty;
            for (int i = 0; i < logs.Count; i++)
            {
                output += $"\n {logs[logs.Count - i - 1]}";
            }
            return output;
        }
    }
}
