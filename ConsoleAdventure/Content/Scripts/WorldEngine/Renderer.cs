using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.Player;
using System.Threading;
using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.IO;
using Microsoft.Xna.Framework.Graphics;
using System.Linq;
using ConsoleAdventure.Content.Scripts.WorldEngine;

namespace ConsoleAdventure.WorldEngine
{
    public class Renderer
    {
        private World world;
        private int viewDistanceY = 30;
        private int viewDistanceX = 60;

        private string[] destroys = new string[]
        {
            "  ",
            "──",
            "xх",
            "XХ",
            "╳╳",
            "╳╳",
        };

        public Renderer()
        {

        }

        int timer;
        Position oldPosition;
        int oldW;
        public void Render(Position observer, int observerW, Position cursorPosition, Color cursorColor)
        {
            world = ConsoleAdventure.world;

            LoadViewChunks(observer);

            ConsoleAdventure.startDisplay = observer - new Position(30, 15);
            ConsoleAdventure.endDisplay = observer + new Position(30, 15);

            SpriteBatch spriteBatch = ConsoleAdventure._spriteBatch;
            SpriteFont font = ConsoleAdventure.Font;
            Vector2 worldPos = ConsoleAdventure.worldPos;
            Vector2 cellSize = ConsoleAdventure.cellSize;

            if (ConsoleAdventure.WindowActive)
            {
                Light.Clear();
                StringPaint.Clear();

                for (int i = -11; i < 61 + 11; i++)
                {
                    for (int j = -11; j < 61 + 11; j++)
                    {
                        int x = i + ConsoleAdventure.startDisplay.x;
                        int y = j + ConsoleAdventure.startDisplay.y;

                        if (ConsoleAdventure.world.GetChunk(x, y, out int v1, out int v2) is LoadedChunk)
                        {
                            Transform t1 = ConsoleAdventure.world.GetField(x, y, World.BlocksLayerId, observerW)?.content;
                            Transform t2 = ConsoleAdventure.world.GetField(x, y, World.MobsLayerId, observerW)?.content;
                            Transform t3 = ConsoleAdventure.world.GetField(x, y, World.ItemsLayerId, observerW)?.content;

                            if (t1 != null) t1.OnTheScreen();
                            if (t2 != null) t2.OnTheScreen();
                            if (t3 != null) t3.OnTheScreen();
                        }
                    }
                }
            }

            spriteBatch.DrawFrame(font, Utils.GetPanel(new(122, 32)), new(worldPos.X - (cellSize.X / 2) + 4, worldPos.Y - cellSize.Y), new Color(50, 50, 50));

            if (observer != oldPosition || observerW != oldW || timer % 5 == 0)
            {
                Light.Update(observer);
            }

            int noClampedStartY = observer.y - viewDistanceY / 2;
            int noClampedStartX = observer.x - viewDistanceX / 2;

            int startY = Math.Clamp(noClampedStartY, 0, world.size);
            int startX = Math.Clamp(noClampedStartX, 0, world.size);
            int endY = Math.Clamp(observer.y + viewDistanceY / 2, 0, world.size);
            int endX = Math.Clamp(observer.x + viewDistanceX / 2, 0, world.size);

            int nx = 0;
            int X = 0;
            int Y = 0;
       
            if (noClampedStartX < 0) { nx = -noClampedStartX; X = nx; }
            if (noClampedStartY < 0) { Y = -noClampedStartY; }

            for (int y = startY; y < endY; y++)
            {
                for (int x = startX; x < endX; x++)
                {
                    Chunk chunk = GetChunk(x, y);
                    Vector3 lightColor = Light.colors[X, Y].ToVector3();

                    for (int z = 0; z < World.CountOfLayers; z++)
                    {
                        Field field = chunk?.GetField(x % Chunk.Size, y % Chunk.Size, z, observerW);

                        if (field?.content != null)
                        {
                            Transform transform = field.content;
                            Vector2 drawPos = new Vector2((X * cellSize.X) + worldPos.X, (Y * cellSize.Y) + worldPos.Y);

                            if (transform.GetBGColor() != null)
                            {
                                spriteBatch.DrawString(font, "██", drawPos, (((Color)transform.GetBGColor()).ToVector3() * lightColor).ToColor());
                            }

                            spriteBatch.DrawString(font, field.GetSymbol(), drawPos, (Color)((transform.GetColor()).ToVector3() * lightColor).ToColor());

                            if (transform.degreeDestruction > 16)
                            {
                                int destroyIndex = (int)(((float)transform.degreeDestruction) / 16);
                                destroyIndex = destroyIndex > 5 ? 5 : destroyIndex;
                                spriteBatch.DrawString(font, destroys[destroyIndex], drawPos, Color.Black);
                            }
                        }

                        if (field == null)
                        {
                            Vector2 drawPos = new Vector2((X * cellSize.X) + worldPos.X, (Y * cellSize.Y) + worldPos.Y);
                            spriteBatch.DrawString(font, " ?", drawPos, new(50, 50, 50));

                        }
                    }
                    X++;
                }
                Y++; X = nx;
            }

            StringPaint.DrawUnits((ConsoleAdventure.startDisplay).ToVector2());

            if (Cursor.Instance != null && Cursor.Instance.IsActive)
            {
                DrawCursor(cursorPosition, cursorColor);
            }

            oldPosition = observer;
            oldW = observerW;
            
            timer++;
        }

        private void DrawCursor(Position cursorPosition, Color cursorColor)
        {
            ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, "><", new Vector2((viewDistanceX * ConsoleAdventure.cellSize.X / 2) + ConsoleAdventure.worldPos.X
                + cursorPosition.x * ConsoleAdventure.cellSize.X, (viewDistanceY * ConsoleAdventure.cellSize.Y / 2) + ConsoleAdventure.worldPos.Y
                + cursorPosition.y * ConsoleAdventure.cellSize.Y), cursorColor);
        }

        private Chunk GetChunk(int x, int y)
        {
            int chunkX = x / Chunk.Size;
            int chunkY = y / Chunk.Size;

            if (chunkX >= 0 && chunkX < world.chunks.GetLength(0) && chunkY >= 0 && chunkY < world.chunks.GetLength(1))
            {
                return world.chunks[chunkX, chunkY];
            }
            return null;
        }


        List<Position> loadedChunks = new();
        Position oldChunkPos = new(-1600, -1600);

        public void LoadViewChunks(Position observer)
        {
            Position chunkPos = observer / Chunk.Size;

            if (chunkPos != oldChunkPos)
            {
                int radius = (ConsoleAdventure.ChunkLoadRadius / 2);
                List<Position> curLoadedChunks = loadedChunks.ToArray().ToList();
                loadedChunks.Clear();

                for (int i = -radius; i <= radius; i++)
                {
                    for (int j = -radius; j <= radius; j++)
                    {
                        int x = i + chunkPos.x;
                        int y = j + chunkPos.y;

                        if (x >= 0 && y >= 0 && x < world.GetChunkCounts().X && y < world.GetChunkCounts().Y)
                        {
                            Chunk chunk = world.chunks[x, y];

                            if (chunk == null || chunk is UnloadedChunk)
                            {
                                world.LoadChunk(x, y, true);
                            }

                            loadedChunks.Add(new(x, y));
                        }
                    }
                }

                for (int i = 0; i < curLoadedChunks.Count; i++)
                {
                    Position position = curLoadedChunks[i];

                    bool isFound = false;

                    for (int j = 0; j < loadedChunks.Count; j++)
                    {
                        if (position == loadedChunks[j])
                        {
                            isFound = true;
                            break;
                        }
                    }

                    if (!isFound)
                    {
                        Chunk chunk = world.chunks[position.x, position.y];

                        if (chunk is LoadedChunk)
                        {
                            world.UnloadChunk(position.x, position.y);
                        }
                    }
                }

                oldChunkPos = chunkPos;
            }
        }
    }
}