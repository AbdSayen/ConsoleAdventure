using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class Loot : Storage
    {
        public Loot(Position position, int w, List<Stack> items, int worldLayer = -1) : base(position, w, AddItems(items, position.x, position.y, World.ItemsLayerId, w))
        {
            type = (int)RenderFieldType.loot;

            AddTypeToMap<Loot>(type);

            if (this.items != null)
                Initialize();
        }

        private static List<Stack> AddItems(List<Stack> items, int x, int y, int z, int w)
        {
            if(items == null)
            {
                return items;
            }

            List<Stack> result = items;
            List<Stack> stacks = new List<Stack>();

            Field field = ConsoleAdventure.world.GetField(x, y, z, w);

            if (field.content != null)
            {
                if (field.content is Loot)
                {
                    stacks = ((Loot)field.content).items;
                }

                if (field.content is Chest)
                {
                    ((Chest)field.content).AddRange(items);
                    return null;
                }
            }

            result.AddRange(stacks);
            return result;
        }

        public void PickUpAll(Inventory inventory)
        {
            world.RemoveSubject(this, World.ItemsLayerId);
            inventory.PickUpItems(items);
        }

        public override string GetSymbol()
        {
            return " $";
        }

        public override Color GetColor()
        {
            return Color.Yellow;
        }
    }
}
