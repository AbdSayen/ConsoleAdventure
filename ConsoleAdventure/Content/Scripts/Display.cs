using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.Player;
using ConsoleAdventure.Content.Scripts.UI;
using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Xml;

namespace ConsoleAdventure
{
    public class Display
    {
        internal static RecipesUI recipesUI;
        public static Bar hpBar;
        public static Bar manaBar;
        public static Bar foodBar;

        private World world;
        private static Vector2 barsPos;

        public Display(World world)
        {
            this.world = world;
        }

        public static void SetBars()
        {
            barsPos = ConsoleAdventure.worldPos + new Vector2(0, -(19 * 2));
            hpBar = new Bar(new Vector2(0, 0), Color.Red, 20);
            hpBar.Position = barsPos + new Vector2(27, 0);
            hpBar.baseSymbol = '∙';
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

            DrawBars();
        }

        public void DrawBars()
        {
            Player player = world.GetLocalPlayer();
            hpBar.Progress = (uint)((float)player.life / player.maxLife * 100);

            ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, "♥", barsPos, Color.Red);
            ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, $"[{player.life}/{player.maxLife}]", barsPos + new Vector2(18, -19), new Color(80, 80, 80));
            hpBar.Draw(ConsoleAdventure._spriteBatch);
        }

        public void DisplayInventory(Vector2 position)
        {
            //return
            //$"{TextAssets.Inventory}\n" +
            //$"{world.GetLocalPlayer().inventory.GetInfo()}\n" +
            //$"{Loger.GetLogs()}"
            //;

            Player player = world.GetLocalPlayer();
            Inventory inventory = player.inventory;
            Vector2 startLogs = DrawItems(inventory, position, player.holdItemIndex);

            if(player.isChestOpen) 
            {
                Vector2 pos = DrawItems(player.chest, position + new Vector2(-260, 0), player.holdChestItemIndex);
                ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, "──────────────────────────\n", pos, Color.White);
            }

            if (player.isCraftOpen)
            {
                recipesUI.Draw(ConsoleAdventure._spriteBatch);
            }

            var buffs = player.buffs;
            Buff curBuff = null;

            if (buffs.Count > 0)
            {
                Vector2 logsOffset = new();

                ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, "──────────────────────────\n", startLogs, Color.White);

                int y = 0;
                int x = 0;
                for (int i = 0; i < buffs.Count; i++)
                {
                    Vector2 buffPosition = startLogs + new Vector2(30 * x, 25 * y + 19);

                    if (player.buffCursor == i)
                    {
                        ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, "˹ ˺", buffPosition + new Vector2(-1, 4), new(255, 255, 0));
                        ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, "˻ ˼", buffPosition + new Vector2(-1, 15), new(255, 255, 0));


                        curBuff = buffs[i];
                    }

                    buffs[i].Draw(ConsoleAdventure._spriteBatch, buffPosition);

                    if (i % 8 == 7)
                    {
                        y++;
                        x = 0;
                        logsOffset += new Vector2(0, 25);
                    }

                    else x++;
                }

                startLogs += new Vector2(0, 19 * 3) + logsOffset;

                if (curBuff != null)
                {
                    int second = curBuff.time / 60;
                    string buffText = "──────────────────────────\n" + curBuff.GetName() + $"({(second > 59 ? (second / 60) + "m" : second + "s")})\n" + curBuff.GetDescription();
                    Vector2 buffTextSize = ConsoleAdventure.Font.MeasureString(buffText);

                    ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, buffText, startLogs, Color.White);

                    startLogs.Y += buffTextSize.Y;
                }
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
