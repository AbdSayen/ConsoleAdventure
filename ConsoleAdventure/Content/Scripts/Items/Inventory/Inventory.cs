﻿using System;
using System.Collections.Generic;
using System.Linq;
using ConsoleAdventure.CaModLoaderAPI;
using ConsoleAdventure.Content.Scripts.Player;

namespace ConsoleAdventure
{
    [Serializable]
    public class Inventory
    {
        private Player player;
        private List<Stack> slots = new List<Stack>();
        private int maxCount = 10;

        public Inventory(Player player)
        {
            this.player = player;
        }

        public void PickUpItems(List<Stack> items)
        {
            Dictionary<Item, int> skippedItems = new Dictionary<Item, int>();

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i].item; 
                if (!item.CanBePickedUp())  // Пропускаем если предмет нельзя поднять
                {
                    if (!skippedItems.Keys.Contains(item))
                        skippedItems[item] = 0;
                    skippedItems[item]++;
                    continue;
                }

                bool itemAdded = false;

                for (int j = 0; j < slots.Count; j++)
                {
                    if (slots[j].item.name == items[i].item.name && slots[j].count < slots[j].maxStackCount)
                    {
                        int availableSpace = slots[j].maxStackCount - slots[j].count;
                        int itemsToAdd = Math.Min(availableSpace, items[i].count);

                        slots[j].AddItems(itemsToAdd);
                        items[i].count -= itemsToAdd;

                        if (items[i].count <= 0)
                        {
                            itemAdded = true;
                            break;
                        }
                    }
                }

                while (!itemAdded && items[i].count > 0)
                {
                    if (slots.Count < maxCount)
                    {
                        int itemsToAdd = Math.Min(items[i].count, items[i].maxStackCount);
                        slots.Add(new Stack(items[i].item, itemsToAdd));
                        items[i].count -= itemsToAdd;

                        if (items[i].count <= 0)
                        {
                            itemAdded = true;
                        }
                    }
                    else
                    {
                        Drop(new List<Stack> { new Stack(items[i].item, items[i].count) });
                        break;
                    }
                }
            }

            foreach (Item itm in skippedItems.Keys)
            {
                Drop(new List<Stack> { new Stack(itm, skippedItems[itm]) });
            }

            for (int i = 0; i < slots.Count; i++)
            {
                if (slots[i].count > slots[i].maxStackCount)
                {
                    int surplus = slots[i].count - slots[i].maxStackCount;
                    slots[i].count = slots[i].maxStackCount;
                    Drop(new List<Stack> { new Stack(slots[i].item, surplus) });
                }
            }
        }

        public void Drop(List<Stack> items)
        {
            new Loot(player.position, ConsoleAdventure.curDeep, items);
        }

        public string GetInfo()
        {
            string output = string.Empty;
            for (int i = 0; i < slots.Count; i++)
            {
                output += $"{slots[i].GetInfo()}\n";
            }
            return output;
        }
        public bool HasItems(Item item, int count)
        {
            int total = 0;
            foreach (var slot in slots)
            {
                if (slot.item.name == item.name)
                {
                    total += slot.count;
                    if (total >= count)
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void RemoveItems(Item item, int count)
        {
            int total = 0;

            if (HasItems(item, count))
            {
                int itemsToRemove = count;
                for (int i = 0; i < slots.Count; i++)
                {
                    if (slots[i].item.name == item.name)
                    {
                        if (slots[i].count <= itemsToRemove)
                        {
                            itemsToRemove -= slots[i].count;
                            slots.RemoveAt(i);
                            i--; // Уменьшаем индекс, так как элемент был удален
                        }
                        else
                        {
                            slots[i].count -= itemsToRemove;
                            break;
                        }
                    }
                }
            }
        }
    }
}