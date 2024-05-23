using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    public class Inventory
    {
        private Player player;
        private List<Stack> slots = new List<Stack>();
        private int maxCount = 10;

        public Inventory(Player player) 
        {
            this.player = player;
            Drop(new List<Stack> { new Stack(new Apple(), 150)});
        }

        public void PickUpItems(List<Stack> items)
        {
            for (int i = 0; i < items.Count; i++)
            {
                bool itemAdded = false;

                // Попробуем добавить предметы в существующие слоты
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

                // Если есть остатки предметов, пробуем добавить в новые слоты
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
                        // Нет свободных слотов, выбрасываем остаток
                        Drop(new List<Stack> { new Stack(items[i].item, items[i].count) });
                        break;
                    }
                }
            }

            // Убедимся, что ни один слот не превышает maxStackCount
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
            new Loot(player.world, player.position, items);
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
    }
}
