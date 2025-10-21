using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.WorldEngine.ChunkManagement.Tasks
{
    public class LoadChunkTask : IChunkTask
    {
        public Position Chunk { get; set; } = new Position(-1, -1);

        public void Execute() 
        {
            ChunkManager.LoadChunk(Chunk.x, Chunk.y, false);
        }
    }
}
