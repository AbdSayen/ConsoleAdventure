﻿using ConsoleAdventure.Settings;

namespace ConsoleAdventure
{
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
            return $"{item.name} x{count}\n{item.description}";
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
