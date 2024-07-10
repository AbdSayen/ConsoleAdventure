using System.Collections.Generic;

namespace ConsoleAdventure
{
    public class Loot : Transform
    {
        private List<Stack> items { get; set; }

        public Loot(WorldEngine.World world, Position position, List<Stack> items, int worldLayer = -1) : base(world, position)
        {
            this.items = items;
            isObstacle = false;
            renderFieldType = WorldEngine.RenderFieldType.loot;

            if (worldLayer == -1) this.worldLayer = WorldEngine.World.BlocksLayerId;
            else this.worldLayer = worldLayer;
            Initialize();
        }

        public void PickUpAll(Inventory inventory)
        {
            inventory.PickUpItems(items);
            world.RemoveSubject(this, WorldEngine.World.BlocksLayerId);
        }

        public string GetItemsInfo()
        {
            string output = string.Empty;

            for (int i = 0; i < items.Count; i++)
            {
                output += $"{items[i].item.name} {items[i].count}\n";
            }

            return output;
        }
    }
}
