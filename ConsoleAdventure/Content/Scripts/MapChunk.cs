using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts
{
    internal struct MapChunk
    {
        public MapField?[,,] fields;

        public MapChunk()
        {
            fields = new MapField?[Chunk.Size, Chunk.Size, Chunk.maxDeep];
        }
    }

    public struct MapField //
    {
        public Color color;

        public MapField(Color color, byte lighting)
        {
            this.color = color;
            this.color.A = lighting;
        }
    }
}
