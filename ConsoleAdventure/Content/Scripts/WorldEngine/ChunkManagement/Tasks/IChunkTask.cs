using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.WorldEngine.ChunkManagement.Tasks
{
    public interface IChunkTask
    {
        public Position Chunk { get; set; }

        public virtual void Execute()
        {

        }
    }
}
