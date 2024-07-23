using System.Collections.Generic;

namespace ConsoleAdventure.Settings
{
    public static class Loger
    {
        static List<string> logs = new List<string>();

        public static int buffer = 50; 

        public static void AddLog(string log)
        {
            logs.Add(log);
            if (logs.Count > buffer)
            {
                logs.RemoveAt(0);
            }
        }

        public static void ClearLogs()
        {
            logs.Clear();
        }

        public static string GetLogs()
        {
            string output = string.Empty;
            if (logs.Count > 0)
            {
                output += "Logs:\n";
            }
            for (int i = 0; i < logs.Count; i++)
            {
                output += $"{logs[logs.Count - i - 1]}\n";
            }
            return output;
        }
    }
}
