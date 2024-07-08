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

        public string Render(Transform observer, int layer)
        {
            StringBuilder output = new StringBuilder();
            for (int y = observer.position.y - viewDistanceY / 2; y < observer.position.y + viewDistanceY / 2; y++)
            {
                if (y >= 0 && y < chunks.Count * Chunk.Size)
                {
                    for (int x = observer.position.x - viewDistanceX / 2; x < observer.position.x + viewDistanceX / 2; x++)
                    {
                        if (x >= 0 && x < chunks[0].Count * Chunk.Size)
                        {
                            var chunk = GetChunk(x, y);
                            var field = chunk?.GetField(x % Chunk.Size, y % Chunk.Size, layer);
                            if (field != null)
                            {
                                output.Append(field.GetSymbol());
                            }
                            else
                            {
                                output.Append("  ");
                            }
                        }
                        else
                        {
                            output.Append("  ");
                        }
                    }
                    output.AppendLine();
                }
                else
                {
                    output.Append(' ', viewDistanceX).AppendLine();
                }
            }
            return output.ToString();
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
    }
}
