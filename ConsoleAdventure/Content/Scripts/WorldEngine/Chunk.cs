using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine
{
    public class Chunk
    {
        protected short biome;
        public bool IsUpdated { get; internal set; } = false;
        public static int Size { get; internal set; } = 16;
        public static int maxDeep { get; internal set; } = 2;

        public virtual Field GetField(int x, int y, int z, int w)
        {
            return null;
        }

        public virtual void SetField(int x, int y, int layer, int w, Field field)
        {
        }

        public virtual short GetBiome()
        {
            return biome;
        }
        public virtual Field[,,,] GetFields()
        {
            return null;
        }

        public bool IsValidCoordinate(int x, int y, int z, int w)
        {
            return x >= 0 && x < Size &&
                   y >= 0 && y < Size &&
                   z >= 0 && z < World.CountOfLayers &&
                   w >= 0 && w < maxDeep;
        }
    }
}
