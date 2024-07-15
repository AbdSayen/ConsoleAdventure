using System;

namespace ConsoleAdventure
{
    [Serializable]
    public class Log : Item
    {
        public Log()
        {
            name = Localization.GetTranslation("Items", GetType().Name);
            description = GetDescription();
        }

        public new string GetDescription()
        {
            return " " + Localization.GetTranslation("ItemDescription", "Log");
        }
    }
}