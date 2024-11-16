using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Chunk
    {
        private readonly Field[,,,] fields;
        private short biome;
        public static int Size = 16;
        public static int maxDeep = 2;

        public Chunk()
        {
            fields = InitializeFields();
        }

        private Field[,,,] InitializeFields()
        {
            var initializedFields = new Field[Size, Size, World.CountOfLayers, maxDeep];

            for (int i = 0; i < Size; i++)
            {
                for (int j = 0; j < Size; j++)
                {
                    for (int k = 0; k < World.CountOfLayers; k++)
                    {
                        for (int l = 0; l < maxDeep; l++)
                        {
                            initializedFields[i, j, k, l] = new Field();
                        }
                    }
                }
            }

            return initializedFields;
        }

        public Field GetField(int x, int y, int z, int w)
        {
            if (IsValidCoordinate(x, y, z, w))
            {
                Field field = fields[x, y, z, w];
                if (field == null)
                {
                    field = new Field();
                }
                return field;
            }
            return null;
        }

        public void SetField(int x, int y, int layer, int w, Field field)
        {
            if (IsValidCoordinate(x, y, layer, w))
            {
                fields[x, y, layer, w] = field;
            }
        }

        public short GetBiome()
        {
            return biome;
        }

        /*public List<List<Field>> GetFields(int z, int w)
        {
            if (w >= 0 && w < fields.Count)
            {
                if (z >= 0 && z < fields[w].Count)
                {
                    return fields[w][z];
                }
            }
            return null;
        }*/

        /*public List<List<List<Field>>> GetFields(int w)
        {
            if (w >= 0 && w < fields.Count)
            {
                return fields[w];
            }
            return null;
        }*/

        public Field[,,,] GetFields()
        {
            return fields;
        }

        private bool IsValidCoordinate(int x, int y, int z, int w)
        {
            return x >= 0 && x < Size &&
                   y >= 0 && y < Size &&
                   z >= 0 && z < World.CountOfLayers &&
                   w >= 0 && w < maxDeep;
        }
    }
}