using SharpDX.Direct2D1;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class UnloadedChunk : Chunk
    {
        public short[,,,] fields;
        public List<TransformDataInChunk> data = new();
        public List<TransformMaterialInChunk> materials = new();

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

    public struct TransformMaterialInChunk
    {
        public Position position = new();
        public byte z;
        public byte w;
        public int material;

        public TransformMaterialInChunk(short x, short y, byte z, byte w, int material)
        {
            position.x = x;
            position.y = y;
            this.z = z;
            this.w = w;
            this.material = material;
        }
    }
}