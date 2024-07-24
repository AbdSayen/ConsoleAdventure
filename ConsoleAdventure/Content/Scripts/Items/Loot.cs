using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    [Serializable]
    public class Loot : Storage
    {
        public Loot(WorldEngine.World world, Position position, List<Stack> items, int worldLayer = -1) : base(world, position, items)
        {
            renderFieldType = WorldEngine.RenderFieldType.loot;
            Initialize();
        }

        public void PickUpAll(Inventory inventory)
        {
            inventory.PickUpItems(items);
            world.RemoveSubject(this, WorldEngine.World.ItemsLayerId);
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
