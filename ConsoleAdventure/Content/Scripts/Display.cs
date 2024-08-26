using ConsoleAdventure.Content.Scripts.UI;
using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System.Xml;

namespace ConsoleAdventure
{
    internal class Display
    {
        internal static RecipesUI recipesUI;
        private World world;
        public Display(World world)
        {
            this.world = world;
        }

        public string DisplayInfo()
        {
            return
                //$"{Docs.GetInfo()}\n" +
                $"{world.time.GetTime()}\n" +
                $"X:{world.GetLocalPlayer().position.x} Y:{world.GetLocalPlayer().position.y}\n" +
                (ConsoleAdventure.isPause ? TextAssets.Paused : "") + "\n\n"
                ;
        }

        public string TransformTooltip()
        {
            Position pos = ConsoleAdventure.MouseWorld;
            return
                $"│ {TextAssets.FloorTooltip + Transform.GetName(pos, 0, ConsoleAdventure.curDeep, true)}\n" +
                $"│ {TextAssets.BlockTooltip + Transform.GetName(pos, 1, ConsoleAdventure.curDeep, true)}\n" +
                $"│ {TextAssets.LootTooltip + Transform.GetName(pos, 2, ConsoleAdventure.curDeep, true)}\n" +
                $"│ {TextAssets.EntityTooltip + Transform.GetName(pos, 3, ConsoleAdventure.curDeep, true)}\n\n"
                ;
        }

        public void DrawWorld()
        {
            world.Render();
        }

        public void DisplayInventory(Vector2 position)
        {
            //return
                //$"{TextAssets.Inventory}\n" +
                //$"{world.GetLocalPlayer().inventory.GetInfo()}\n" +
                //$"{Loger.GetLogs()}"
                //;

            Inventory inventory = world.GetLocalPlayer().inventory;
            Vector2 startLogs = DrawItems(inventory, position, world.GetLocalPlayer().holdItemIndex);

            if(world.GetLocalPlayer().isChestOpen) 
            {
                Vector2 pos = DrawItems(world.GetLocalPlayer().chest, position + new Vector2(-260, 0), world.GetLocalPlayer().holdChestItemIndex);
                ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, "──────────────────────────\n", pos, Color.White);
            }

            if (ConsoleAdventure.world.GetLocalPlayer().isCraftOpen)
            {
                recipesUI.Draw(ConsoleAdventure._spriteBatch);
            }

            ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, "──────────────────────────\n" + Loger.GetLogs(), startLogs , Color.White);
        }

        private Vector2 DrawItems(Inventory inventory, Vector2 position, int cursor)
        {
            Vector2 startDescription = position + new Vector2(0, 19 * inventory.slots.Count);
            Vector2 startLogs = startDescription;

            for (int i = 0; i < inventory.slots.Count; i++)
            {
                inventory.slots[i].item.Draw(ConsoleAdventure._spriteBatch, position + new Vector2(0, i * 19));

                Color color = Color.White;

                if (cursor == i)
                {
                    color = Color.Yellow;
                    ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, ">", position + new Vector2(-18, i * 19), color);
                    string description = "──────────────────────────\n" + inventory.slots[i].item.description;
                    ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, description, startDescription, Color.White);
                    startLogs.Y += ConsoleAdventure.Font.MeasureString(description).Y;

                }

                ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, inventory.slots[i].item.name + (inventory.slots[i].count > 1 ? $" ({inventory.slots[i].count})" : ""), position + new Vector2(18, i * 19), color);
            }

            return startLogs;
        }
    }
}
