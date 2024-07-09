using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.Text;

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

        public void Render(Transform observer)
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
                            for(int z = 0; z < World.CountOfLayers; z++)
                            {
                                var field = chunk?.GetField(x % Chunk.Size, y % Chunk.Size, z);

                                if (field != null)
                                {
                                ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, field.GetSymbol(), new Vector2(X * 18, (Y * 19) + 155), field.color);
                                }
                            }                            
                        }
                        X++;
                    }
                }
                Y++;
                X = 0;
            }
        }

        public string PrimitiveRender()
        {
            StringBuilder output = new StringBuilder();
            for (int y = 0; y < chunks.Count * Chunk.Size; y++)
            {
                for (int x = 0; x < chunks[0].Count * Chunk.Size; x++)
                {
                    var chunk = GetChunk(x, y);
                    var field = chunk?.GetField(x % Chunk.Size, y % Chunk.Size, World.BlocksLayerId);
                    output.Append(field?.GetSymbol() ?? " ");
                }
                output.AppendLine();
            }
            return output.ToString();
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

        /*public string Render(Transform observer, int layer)
        {
            string output = "";

            int x = 0, y = 0;
            for (int i = observer.position.y - viewDistanceY / 2; i < observer.position.y + viewDistanceY / 2; i++) //y
            {
                if (i <= fields[World.FloorLayerId].Count - 1 && i >= 0)
                {
                    for (int j = observer.position.x - viewDistanceX / 2; j < observer.position.x + viewDistanceX / 2; j++) //x
                    {
                        if (j <= fields[World.FloorLayerId][i].Count - 1 && j >= 0)
                        {
                            if (fields[World.MobsLayerId][i][j] != null && layer == World.MobsLayerId)
                            {
                                ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, fields[World.MobsLayerId][i][j].GetSymbol(), new Vector2(x * 18, (y * 19) + 155), Color.Yellow);
                            }
                            else if (fields[World.ItemsLayerId][i][j] != null && layer == World.ItemsLayerId)
                            {
                                ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, fields[World.ItemsLayerId][i][j].GetSymbol(), new Vector2(x * 18, (y * 19) + 155), fields[World.ItemsLayerId][i][j].color);
                            }
                            else if (fields[World.BlocksLayerId][i][j] != null && layer == World.BlocksLayerId)
                            {
                                ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, fields[World.BlocksLayerId][i][j].GetSymbol(), new Vector2(x * 18, (y * 19) + 155), fields[World.BlocksLayerId][i][j].color);
                            }
                            else if (fields[World.FloorLayerId][i][j] != null && layer == World.FloorLayerId)
                            {
                                ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, fields[World.FloorLayerId][i][j].GetSymbol(), new Vector2(x * 18, (y * 19) + 155), fields[World.FloorLayerId][i][j].color);
                            }
                        }
                        else
                        {
                            ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, " `", new Vector2(x * 18, (y * 19) + 155), Color.Yellow);
                        }
                        x++;
                    }
                }
                else
                {
                    for (int k = 0; k < viewDistanceX; k++)
                    {
                        ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, " `", new Vector2(x * 18, (y * 19) + 155), Color.Yellow);
                        x++;
                    }

                }

                y++;
                x = 0;
            }

            return output;
        }

        public string PrimitiveRender()
        {
            StringBuilder output = new StringBuilder();
            for (int y = 0; y < fields[World.FloorLayerId].Count; y++)
            {
                for (int x = 0; x < fields[World.FloorLayerId][y].Count; x++)
                {
                    output.Append(fields[World.BlocksLayerId][y][x].GetSymbol());
                }
                output.AppendLine();
            }
            return output.ToString();
        }*/
    }
}
