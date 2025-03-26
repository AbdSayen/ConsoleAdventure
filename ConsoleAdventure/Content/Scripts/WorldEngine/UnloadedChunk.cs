using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class UnloadedChunk : Chunk
    {
        public short[,,,] fields;
        public List<TransformDataInChunk> data = new();

        public UnloadedChunk()
        {
            fields = new short[Size, Size, World.CountOfLayers, maxDeep];
        }
    }

    public struct TransformDataInChunk
    {
        public Position position = new();
        public byte z;
        public byte w;
        public object data;

        public TransformDataInChunk(short x, short y, byte z, byte w, object data)
        {
            position.x = x;
            position.y = y;
            this.z = z;
            this.w = w;
            this.data = data;
        }
    }
}