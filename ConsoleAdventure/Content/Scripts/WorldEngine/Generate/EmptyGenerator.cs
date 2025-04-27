using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine.Generate
{
    public class EmptyGenerator
    {
        public World world;

        public string processHint
        {
            get
            {
                return processHint;
            }
            set
            {
                ConsoleAdventure.progressBar.stepText = value;
            }
        }
        public int processProgress
        {
            get
            {
                return processProgress;
            }
            set
            {
                ConsoleAdventure.progressBar.Progress = (uint)value;
            }
        }

        // cx, cy = Левый верхний угол чанка
        // setBlock = метод для установки блока или чего либо еще
        internal void ChunkGeneration(int cx, int cy, Action<int, int> setBlock)
        {
            for (int i = 0; i < Chunk.Size; i++)
            {
                for (int j = 0; j < Chunk.Size; j++)
                {
                    setBlock(cx * Chunk.Size + i, cy * Chunk.Size + j);
                }
            }
        }

        internal void IterateWorldChunkByChunk(Action<int, int> setBlock)
        {
            int chunksInWorld = (world.size / Chunk.Size);

            for (int ci = 0; ci < world.size / Chunk.Size; ci++)
            {
                for (int cj = 0; cj < world.size / Chunk.Size; cj++)
                {
                    ChunkGeneration(ci, cj, setBlock);
                    processProgress = (int)((float)(ci * chunksInWorld + cj) / (chunksInWorld * chunksInWorld) * 100);
                }
            }
        }

        public virtual async Task Generate(World world)
        {
            this.world = world;

            processHint = "Generate something?..";
            processProgress = 0;
        }
    }
}
