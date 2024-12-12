using ConsoleAdventure.Content.Scripts.IO;
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
        protected List<Stack> items { get; set; } = new();

        public Storage(Position position, int w, List<Stack> items, int worldLayer = -1) : base(position, (byte)w)
        {
            this.items = items;
            isObstacle = false;
        }

        public List<Stack> GetItems()
        {
            return items;
        }

        public void AddRange(List<Stack> stacks)
        {
            items.AddRange(stacks);
        }

        public string GetItemsInfo()
        {
            string output = string.Empty;

            for (int i = 0; i < items.Count; i++)
            {
                output += $"{items[i].Item.name} {items[i].count}\n";
            }

            return output;
        }

        public override object SaveData()
        {
            return items;
        }

        public override void LoadData(object data)
        {
            if (data is List<Stack>) 
            { 
                items = (List<Stack>)data; 
            }
        }
    }
}
