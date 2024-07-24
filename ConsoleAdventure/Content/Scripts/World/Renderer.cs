using System;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.Player;

namespace ConsoleAdventure.WorldEngine
{
    public class Renderer
    {
        List<List<Chunk>> chunks;
        private int viewDistanceY = 30;
        private int viewDistanceX = 60;

        public Renderer(List<List<Chunk>> chunks)
        {
            this.chunks = chunks;
        }

        public void Render(Transform observer, Position cursorPosition)
        {
            int X = 0, Y = 0;

            ConsoleAdventure._spriteBatch.DrawFrame(ConsoleAdventure.Font, Utils.GetPanel(new(122, 32)), new(ConsoleAdventure.worldPos.X - (ConsoleAdventure.cellSize.X / 2) + 4, ConsoleAdventure.worldPos.Y - ConsoleAdventure.cellSize.Y), new Color(50, 50, 50));

            for (int y = observer.position.y - viewDistanceY / 2; y < observer.position.y + viewDistanceY / 2; y++)
            {
                if (y >= 0 && y < chunks.Count * Chunk.Size)
                {
                    for (int x = observer.position.x - viewDistanceX / 2; x < observer.position.x + viewDistanceX / 2; x++)
                    {
                        if (x >= 0 && x < chunks[0].Count * Chunk.Size)
                        {
                            var chunk = GetChunk(x, y);
                            for (int z = 0; z < World.CountOfLayers; z++)
                            {
                                var field = chunk?.GetField(x % Chunk.Size, y % Chunk.Size, z);

                                if (field != null)
                                {
                                    ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, field.GetSymbol(), new Vector2((X * ConsoleAdventure.cellSize.X) + ConsoleAdventure.worldPos.X, (Y * ConsoleAdventure.cellSize.Y) + ConsoleAdventure.worldPos.Y), field.color);
                                }
                            }
                        }
                        X++;
                    }
                }
                Y++;
                X = 0;
            }
            
            if (Cursor.Instance != null && Cursor.Instance.IsVisible)
            {
                DrawCursor(cursorPosition);
            }
        }

        private void DrawCursor(Position cursorPosition)
        {
            ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, "><", new Vector2((viewDistanceX * ConsoleAdventure.cellSize.X / 2) + ConsoleAdventure.worldPos.X
                + cursorPosition.x * ConsoleAdventure.cellSize.X, (viewDistanceY * ConsoleAdventure.cellSize.Y / 2) + ConsoleAdventure.worldPos.Y
                + cursorPosition.y * ConsoleAdventure.cellSize.Y), Color.Gray);
        }

        private Chunk GetChunk(int x, int y)
        {
            int chunkX = x / Chunk.Size;
            int chunkY = y / Chunk.Size;
            if (chunkX >= 0 && chunkX < chunks.Count && chunkY >= 0 && chunkY < chunks[chunkX].Count)
            {
                return chunks[chunkX][chunkY];
            }
            return null;
        }
    }
}