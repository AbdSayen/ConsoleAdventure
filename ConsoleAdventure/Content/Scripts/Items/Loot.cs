using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class Loot : Storage
    {
        public Loot(Position position, int w, List<Stack> items, int worldLayer = -1) : base(position, w, items)
        {
            type = (int)RenderFieldType.loot;

            AddTypeToMap<Loot>(type);

            Initialize();
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
