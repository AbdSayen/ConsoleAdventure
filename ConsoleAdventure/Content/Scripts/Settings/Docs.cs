using System.Collections.Generic;

namespace ConsoleAdventure.Settings
{
    public static class Docs
    {
        public static string version = "0.2v";
        private static string info;
        private static List<string> messages = new List<string>();
        public static string GetInfo()
        {
            info = $"version: {version}";
            return info;
        }

        public static void AddMessage(MessageType type, string message)
        {
            messages.Add(message);
        }
    }
}
