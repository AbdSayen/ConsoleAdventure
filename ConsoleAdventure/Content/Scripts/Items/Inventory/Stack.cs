using ConsoleAdventure.Settings;
using System;

namespace ConsoleAdventure
{
    [Serializable]
    public class Stack
    {
        private Item item { get; set; }

        public Item Item 
        {
            get
            {
                return item;
            } 
            
            private set
            {
                item = value;
                maxStackCount = item.maxCount;
            } 
        }

        public int count { get; set; }

        public int maxStackCount { get; private set; } = 50;

        public Stack(Item item, int count = 1)
        {
            this.Item = item;
            this.count = count;
        }

        public Stack(Item item, int count = 1, int material = -1)
        {
            this.Item = item;
            this.count = count;

            this.Item.ApplyMaterial(material);
        }

        public string GetInfo()
        {
            return $"{((Item)Item).name}" + (count > 1 ? $" ({count})" : "");
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
            if (Item != null)
            {
                copy.Item = Item.Copy();
            }

            return copy;
        }
    }
}
