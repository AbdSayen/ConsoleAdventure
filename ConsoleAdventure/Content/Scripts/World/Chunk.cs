using SharpDX.Direct2D1;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Chunk
    {
        private readonly List<List<List<List<Field>>>> fields;
        private short biome;
        public static int Size = 16;
        public static int maxDeep = 2;

        public Chunk()
        {
            fields = InitializeFields();
        }

        private List<List<List<List<Field>>>> InitializeFields()
        {
            var initializedFields = new List<List<List<List<Field>>>>(World.CountOfLayers);

            for (int w = 0; w < maxDeep; w++)
            {
                var vertical = new List<List<List<Field>>>(maxDeep);
                for (int z = 0; z < World.CountOfLayers; z++)
                {
                    var layer = new List<List<Field>>(Size);
                    for (int y = 0; y < Size; y++)
                    {
                        var row = new List<Field>(Size);
                        for (int x = 0; x < Size; x++)
                        {
                            row.Add(new Field());
                        }
                        layer.Add(row);
                    }
                    vertical.Add(layer);
                }
                initializedFields.Add(vertical);
            }

            return initializedFields;
        }

        public Field GetField(int x, int y, int z, int w)
        {
            if (IsValidCoordinate(x, y, z, w))
            {
                var vertical = fields[w];
                var layer = vertical[z];
                var row = layer[y];
                var field = row[x];
                if (field == null)
                {
                    field = new Field();
                    row[x] = field;
                }
                return field;
            }
            return null;
        }

        public void SetField(int x, int y, int layer, int w, Field field)
        {
            if (IsValidCoordinate(x, y, layer, w))
            {
                fields[w][layer][y][x] = field;
            }
        }

        public short GetBiome()
        {
            return biome;
        }

        public List<List<Field>> GetFields(int z, int w)
        {
            if (w >= 0 && w < fields.Count)
            {
                if (z >= 0 && z < fields[w].Count)
                {
                    return fields[w][z];
                }
            }
            return null;
        }

        public List<List<List<Field>>> GetFields(int w)
        {
            if (w >= 0 && w < fields.Count)
            {
                return fields[w];
            }
            return null;
        }

        public List<List<List<List<Field>>>> GetFields()
        {
            return fields;
        }

        private bool IsValidCoordinate(int x, int y, int z, int w)
        {
            return z >= 0 && z < fields[w].Count &&
                   y >= 0 && y < fields[w][z].Count &&
                   x >= 0 && x < fields[w][z][y].Count;
        }
    }
}