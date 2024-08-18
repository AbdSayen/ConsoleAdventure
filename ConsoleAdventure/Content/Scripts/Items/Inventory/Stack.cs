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
            return $"{((Item)item).name} ({count})";
        }

        public void AddItems(int count = 1)
        {
            if (count < maxStackCount)
            {
                this.count += count;
            }
        }

        public Stack Copy()
        {
            Stack copy = (Stack)MemberwiseClone();
            if (item != null)
            {
                copy.item = item;
            }

            return copy;
        }
    }
}
