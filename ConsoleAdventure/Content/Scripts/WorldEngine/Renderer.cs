using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.Player;
using System.Threading;
using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.IO;

namespace ConsoleAdventure.WorldEngine
{
    public class Renderer
    {
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
        public void Render(Transform observer, Position cursorPosition, Color cursorColor)
        {
            ConsoleAdventure.startDisplay = observer.position - new Position(30, 15);
            ConsoleAdventure.endDisplay = observer.position + new Position(30, 15);

            int X = 0, Y = 0;

            Light.Clear();
            StringPaint.Clear();

            for (int i = -11; i < 61 + 11; i++)
            {
                for (int j = -11; j < 61 + 11; j++)
                {
                    Field field = ConsoleAdventure.world.GetField(i + ConsoleAdventure.startDisplay.x, j + ConsoleAdventure.startDisplay.y, World.BlocksLayerId, observer.w);
                    Field field1 = ConsoleAdventure.world.GetField(i + ConsoleAdventure.startDisplay.x, j + ConsoleAdventure.startDisplay.y, World.MobsLayerId, observer.w);

                    if (field?.content != null)
                    {
                        field.content.OnTheScreen();
                    }

                    if (field1?.content != null)
                    {
                        field1.content.OnTheScreen();
                    }
                }
            }

            ConsoleAdventure._spriteBatch.DrawFrame(ConsoleAdventure.Font, Utils.GetPanel(new(122, 32)), new(ConsoleAdventure.worldPos.X - (ConsoleAdventure.cellSize.X / 2) + 4, ConsoleAdventure.worldPos.Y - ConsoleAdventure.cellSize.Y), new Color(50, 50, 50));
            
            if (observer.position != oldPosition || observer.w != oldW)
                Light.Update(observer.position);

            else if (timer % 5 == 0)
                Light.Update(observer.position);

            for (int y = observer.position.y - viewDistanceY / 2; y < observer.position.y + viewDistanceY / 2; y++)
            {
                if (y >= 0 && y < ConsoleAdventure.world.chunks.GetLength(1) * Chunk.Size)
                {
                    for (int x = observer.position.x - viewDistanceX / 2; x < observer.position.x + viewDistanceX / 2; x++)
                    {
                        if (x >= 0 && x < ConsoleAdventure.world.chunks.GetLength(0) * Chunk.Size)
                        {
                            var chunk = GetChunk(x, y);
                            Vector3 lightColor = Light.colors[X, Y].ToVector3();
                            for (int z = 0; z < World.CountOfLayers; z++)
                            {
                                var field = chunk?.GetField(x % Chunk.Size, y % Chunk.Size, z, observer.w);

                                if (field != null && field.content != null)
                                {

                                    if (field.content?.GetBGColor() != null)
                                    {
                                        ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, "██", new Vector2((X * ConsoleAdventure.cellSize.X) + ConsoleAdventure.worldPos.X, (Y * ConsoleAdventure.cellSize.Y) + ConsoleAdventure.worldPos.Y), (((Color)field.content.GetBGColor()).ToVector3() * lightColor).ToColor());
                                    }

                                    ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, field.GetSymbol(), new Vector2((X * ConsoleAdventure.cellSize.X) + ConsoleAdventure.worldPos.X, (Y * ConsoleAdventure.cellSize.Y) + ConsoleAdventure.worldPos.Y), (Color)((field.content.GetColor()).ToVector3() * lightColor).ToColor());
                                
                                    if(field.content.degreeDestruction > 16)
                                    {
                                        int destroyIndex = (int)(((float)field.content.degreeDestruction) / 16);
                                        destroyIndex = destroyIndex > 5 ? 5 : destroyIndex;
                                        ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, destroys[destroyIndex], new Vector2((X * ConsoleAdventure.cellSize.X) + ConsoleAdventure.worldPos.X, (Y * ConsoleAdventure.cellSize.Y) + ConsoleAdventure.worldPos.Y), Color.Black);
                                    }
                                }
                            }
                        }
                        X++;
                    }
                }
                Y++;
                X = 0;
            }

            StringPaint.DrawUnits((ConsoleAdventure.startDisplay).ToVector2());

            if (Cursor.Instance != null && Cursor.Instance.IsActive)
            {
                DrawCursor(cursorPosition, cursorColor);
            }

            oldPosition = observer.position;
            oldW = observer.w;
            
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
            if (chunkX >= 0 && chunkX < ConsoleAdventure.world.chunks.GetLength(0) && chunkY >= 0 && chunkY < ConsoleAdventure.world.chunks.GetLength(1))
            {
                return ConsoleAdventure.world.chunks[chunkX, chunkY];
            }
            return null;
        }
    }
}