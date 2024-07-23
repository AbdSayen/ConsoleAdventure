using ConsoleAdventure.Settings;
using System;

namespace ConsoleAdventure
{
    [Serializable]
    public class Stack
    {
        public Item item { get; private set; }
        public int count { get; set; }
        public int maxStackCount { get; private set; } = 50;
        public Stack(Item item, int count = 1)
        {
            this.item = item;
            this.count = count;
        }

        public string GetInfo()
        {
            return $"{((Item)item).name} x{count}\n{((Item)item).description}";
        }

        public void AddItems(int count = 1)
        {
            if (count < maxStackCount)
            {
                this.count += count;
            }
        }
    }
}
