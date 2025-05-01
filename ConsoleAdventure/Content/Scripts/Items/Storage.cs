using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.WorldEngine;
using SharpDX.Direct2D1;
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
            if (items != null)
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

        public override string ModifyTooltip()
        {
            string text = degreeDestruction > 0 ? $" ({100 - degreeDestruction} / 100)" : "";

            if (items != null && items?.Count > 0)
            {
                StringBuilder stringBuilder = new StringBuilder();
                int width = Math.Min(items.Count, 5);
                for (int i = 0; i < width; i++)
                {
                    stringBuilder.Append($"[item:{items[i].Item.GetType().FullName}] ");
                    stringBuilder.Append($"{items[i].GetInfo()}");
                    if (i != width - 1)
                    {
                        stringBuilder.Append(", ");
                    }
                }

                text += " [" + stringBuilder.ToString() + (GetItems().Count > 5 ? "..." : "") + "]";
            }

            return Localization.GetTranslation("Transforms", GetType().Name) + text;
        }
    }
}
