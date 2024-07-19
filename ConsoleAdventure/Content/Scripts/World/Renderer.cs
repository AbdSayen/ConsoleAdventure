using Microsoft.Xna.Framework;
using System.Collections.Generic;

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

        public void Render(Transform observer, Position cursorPosition = null)
        {
            int X = 0, Y = 0;

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
                                    ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, field.GetSymbol(), new Vector2(X * ConsoleAdventure.cellSize, (Y * ConsoleAdventure.cellSize) + 150), field.color);
                                }
                            }
                        }
                        X++;
                    }
                }
                Y++;
                X = 0;
            }

            if (cursorPosition != null)
            {
                ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, "[]", new Vector2((viewDistanceX * ConsoleAdventure.cellSize / 2)
                    + cursorPosition.x * ConsoleAdventure.cellSize, (viewDistanceY * ConsoleAdventure.cellSize / 2) + 150
                    + cursorPosition.y * ConsoleAdventure.cellSize), Color.Gray);
            }
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