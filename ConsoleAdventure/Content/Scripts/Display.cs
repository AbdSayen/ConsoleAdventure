using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.InputLogic;
using ConsoleAdventure.Content.Scripts.Player;
using ConsoleAdventure.Content.Scripts.UI;
using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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

        public bool mapFlag = false;
        public Point mapPos = new Point();
        public bool zoom = true;
        private Dictionary<Point, MapChunk> mapBuffer = new();
        public int mapW = 1;

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
                $"X:{world.GetLocalPlayer().position.x} Y:{world.GetLocalPlayer().position.y} W:{world.GetLocalPlayer().w}\n" +
                (ConsoleAdventure.isPause ? TextAssets.Paused : "") + "\n\n"
                ;
        }

        public string TransformTooltip()
        {
            Position pos = ConsoleAdventure.MouseWorld;
            int w = world.GetLocalPlayer().w;
            return
                $"│ {TextAssets.FloorTooltip + Transform.GetTooltip(pos, 0, w)}\n" +
                $"│ {TextAssets.BlockTooltip + Transform.GetTooltip(pos, 1, w)}\n" +
                $"│ {TextAssets.LootTooltip + Transform.GetTooltip(pos, 2, w)}\n" +
                $"│ {TextAssets.EntityTooltip + Transform.GetTooltip(pos, 3, w)}\n\n"
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

        int timer;

        public void DrawMap()
        {
            //mapFlag = false;
            Player player = world.GetLocalPlayer();

            if (Input.PostClick(InputConfig.MapOpen) && !ConsoleAdventure.BlockHotKey)
            {
                if (mapFlag) mapFlag = false;
                else if (!mapFlag) { mapFlag = true; mapW = world.GetLocalPlayer().w; }
            }

            if (mapFlag)
            {
                ConsoleAdventure._spriteBatch.Draw(ConsoleAdventure.pixel, Vector2.Zero, null, Color.Black, 0, Vector2.Zero, new Vector2(ConsoleAdventure.Width, ConsoleAdventure.Height), 0, 1);

                if (Input.PostClick(InputConfig.NavigationUp)) mapPos.Y--;
                if (Input.PostClick(InputConfig.NavigationDown)) mapPos.Y++;
                if (Input.PostClick(InputConfig.NavigationLeft)) mapPos.X--;
                if (Input.PostClick(InputConfig.NavigationRight)) mapPos.X++;

                if (Input.PostClick(InputConfig.MapZoom) && !ConsoleAdventure.BlockHotKey)
                {
                    if (zoom) zoom = false;
                    else if (!zoom) zoom = true;
                }

                if (Input.PostClick(InputConfig.MapWUp) && !ConsoleAdventure.BlockHotKey && mapW < Chunk.maxDeep - 1) mapW++;
                else if (Input.PostClick(InputConfig.MapWDown) && !ConsoleAdventure.BlockHotKey && mapW > 0) mapW--;

                if (zoom)
                {

                    if (timer % 10 == 0)
                    {
                        mapBuffer.Clear();
                        /*for (int i = 0; i < player.map.data.Count; i++)
                        {
                            var chunk = player.map.data.ElementAt(i);
                            Point pos = chunk.Key;

                            if (pos.X >= mapPos.X && pos.X <= mapPos.X + 6 && pos.Y >= mapPos.Y && pos.Y <= mapPos.Y + 3)
                            {
                                mapBuffer.Add(pos, chunk.Value);
                            }
                        }*/
                    }

                    for (int g = 0; g < mapBuffer.Count; g++)
                    {
                        var chunk = mapBuffer.ElementAt(g);

                        for (int i = 0; i < 16; i++)
                        {
                            for (int j = 0; j < 16; j++)
                            {
                                MapField? mapField = chunk.Value.fields[i, j, mapW];
                                if (mapField != null)
                                {
                                    byte l = mapField.Value.color.A;
                                    ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, "██", new(((chunk.Key.X - mapPos.X) * 18 * 16) + (i * 18), ((chunk.Key.Y - mapPos.Y) * 19 * 16) + (j * 19)), (mapField.Value.color.ToVector3() * (new Color(l, l, l)).ToVector3()).ToColor());
                                }
                            }
                        }
                    }

                    if (player.w == mapW)
                        ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, "☻", new((player.position.x - (mapPos.X * 16)) * 18, (player.position.y - (mapPos.Y * 16)) * 19), Color.Yellow);
                }

                else
                {
                    if (timer % 10 == 0)
                    {
                        mapBuffer.Clear();
                        for (int i = 0; i < player.map.data.Count; i++)
                        {
                            var chunk = player.map.data.ElementAt(i);
                            Point pos = chunk.Key;

                            if (pos.X >= mapPos.X && pos.X <= mapPos.X + 8 * 16 && pos.Y >= mapPos.Y && pos.Y <= mapPos.Y + 4 * 16)
                            {
                                mapBuffer.Add(pos, chunk.Value);
                            }
                        }
                    }

                    for (int g = 0; g < mapBuffer.Count; g++)
                    {
                        var chunk = mapBuffer.ElementAt(g);

                        MapField? mapField = chunk.Value.fields[0, 0, mapW];
                        if (mapField != null)
                        {
                            byte l = mapField.Value.color.A;
                            ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, "██", new(((chunk.Key.X - mapPos.X) * 18), ((chunk.Key.Y - mapPos.Y) * 19)), (mapField.Value.color.ToVector3() * (new Color(l, l, l)).ToVector3()).ToColor());
                        }
                    }
                    if (player.w == mapW)
                        ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, "☻", new(((player.position.x / 16) - mapPos.X) * 18, ((player.position.y / 16) - mapPos.Y) * 19), Color.Yellow);
                }

                string text = $"X:{mapPos.X * 16} Y:{mapPos.Y * 16} W:{mapW} ";
                ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, text, new(ConsoleAdventure.Width - ConsoleAdventure.Font.MeasureString(text).X, 0), Color.White);
                string text1 = (zoom ? "1x1 " : "1x16 ");
                ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, text1, new(ConsoleAdventure.Width - ConsoleAdventure.Font.MeasureString(text1).X, 19), Color.White);

                if (Input.PostClick(Keys.F12))
                {
                    try
                    {
                        System.Drawing.Bitmap bitmap = MapScreen(mapW);
                        if (!Directory.Exists("Screens")) Directory.CreateDirectory("Screens");
                        int count = Directory.GetFiles("Screens", "*.png").Length;

                        bitmap.Save($"Screens/map{count}.png");
                    }

                    catch (Exception ex)
                    {
                    }
                }

                else if (Input.PostClick(Keys.F11))
                {
                    try
                    {
                        System.Drawing.Bitmap bitmap = MapScreen2(mapW);
                        if (!Directory.Exists("Screens")) Directory.CreateDirectory("Screens");
                        int count = Directory.GetFiles("Screens", "*.png").Length;

                        bitmap.Save($"Screens/map{count}.png");
                    }

                    catch (Exception ex)
                    {
                    }
                }
            }

            else
            {
                mapPos = ((player.position / 16) - new Position(1, 1)).ToPoint();
            }

            timer++;
        }

        public System.Drawing.Bitmap MapScreen(int w)
        {
            int size = world.size;

            System.Drawing.Bitmap screen = new System.Drawing.Bitmap(size, size);

            Player player = ConsoleAdventure.world.GetLocalPlayer();

            for (int g = 0; g < player.map.data.Count; g++)
            {
                var chunk = player.map.data.ElementAt(g);

                for (int i = 0; i < 16; i++)
                {
                    for (int j = 0; j < 16; j++)
                    {
                        int x = ((chunk.Key.X * 16) + i);
                        int y = ((chunk.Key.Y * 16) + j);

                        MapField? mapField = chunk.Value.fields[i, j, w];

                        if (mapField != null)
                        {
                            byte l = mapField.Value.color.A;
                            Color color = (Color)(mapField.Value.color.ToVector3() * (new Color(l, l, l)).ToVector3()).ToColor();
                            screen.SetPixel(x, y, System.Drawing.Color.FromArgb(color.A, color.R, color.G, color.B));
                        }

                        else
                        {
                            screen.SetPixel(x, y, System.Drawing.Color.FromArgb(255, 0, 0, 0));
                        }
                    }
                }
            }

            return screen;
        }

        public System.Drawing.Bitmap MapScreen2(int w)
        {
            int size = world.size / Chunk.Size;
            System.Drawing.Bitmap screen = new System.Drawing.Bitmap(size, size);

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    world.GenerateChunk(i, j);

                    Field f = world.chunks[i, j].GetField(0, 0, World.BlocksLayerId, w);

                    if(f?.content != null)
                    {
                        Color color = f.content.GetColor();
                        screen.SetPixel(i, j, System.Drawing.Color.FromArgb(255, color.R, color.G, color.B));
                    }

                    else
                    {
                        screen.SetPixel(i, j, System.Drawing.Color.FromArgb(255, 0, 0, 0));
                    }

                    world.chunks[i, j] = null;
                }
            }

            return screen;
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

            try
            {
                ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, "──────────────────────────\n" + Loger.GetLogs(), startLogs, Color.White);
            }
            catch
            {

            }

            DrawMap();
        }

        private Vector2 DrawItems(Inventory inventory, Vector2 position, int cursor)
        {
            Vector2 startDescription = position + new Vector2(0, 19 * inventory.slots.Count);
            Vector2 startLogs = startDescription;

            for (int i = 0; i < inventory.slots.Count; i++)
            {
                inventory.slots[i].Item.Draw(ConsoleAdventure._spriteBatch, position + new Vector2(0, i * 19));

                Color color = Color.White;

                if (cursor == i)
                {
                    color = Color.Yellow;
                    ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, ">", position + new Vector2(-18, i * 19), color);
                    string description = "──────────────────────────\n" + inventory.slots[i].Item.description;
                    ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, description, startDescription, Color.White);
                    startLogs.Y += ConsoleAdventure.Font.MeasureString(description).Y;

                }

                ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, inventory.slots[i].Item.name + (inventory.slots[i].count > 1 ? $" ({inventory.slots[i].count})" : ""), position + new Vector2(18, i * 19), color);
            }

            return startLogs;
        }
    }
}
