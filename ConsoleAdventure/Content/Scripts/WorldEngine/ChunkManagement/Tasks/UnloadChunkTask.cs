using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.WorldEngine.ChunkManagement.Tasks
{
    public class UnloadChunkTask : IChunkTask
    {
        public Position Chunk { get; set; } = new Position(-1, -1);

        public void Execute()
        {
            ChunkManager.UnloadChunk(Chunk.x, Chunk.y);
        }
    }
}
