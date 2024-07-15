using System;

namespace ConsoleAdventure
{
    [Serializable]
    public abstract class Item
    {
        public string name = "Name missing";
        public string description = "Description missing";
        
        protected string GetDescription()
        {
            return description;
        }
    }
}
