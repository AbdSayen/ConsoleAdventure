using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure
{
    [Serializable]
    public abstract class Storage : Transform
    {
        protected List<Stack> items { get; set; }

        public Storage(Position position, int w, List<Stack> items, int worldLayer = -1) : base(position, w)
        {
            this.items = items;
            isObstacle = false;

            if (worldLayer == -1) this.worldLayer = WorldEngine.World.ItemsLayerId;
            else this.worldLayer = worldLayer;
        }

        public List<Stack> GetItems()
        {
            return items;
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
