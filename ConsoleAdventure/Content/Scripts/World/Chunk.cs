using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class Chunk
    {
        private readonly List<List<List<Field>>> fields;
        private string biome;
        public static int Size = 16;

        public Chunk()
        {
            fields = InitializeFields();
        }

        private List<List<List<Field>>> InitializeFields()
        {
            var initializedFields = new List<List<List<Field>>>(World.CountOfLayers);

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
                initializedFields.Add(layer);
            }

            return initializedFields;
        }

        public Field GetField(int x, int y, int z)
        {
            if (IsValidCoordinate(x, y, z))
            {
                var layer = fields[z];
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

        public string GetBiome()
        {
            return biome ?? "none";
        }

        public List<List<Field>> GetFields(int z)
        {
            if (z >= 0 && z < fields.Count)
            {
                return fields[z];
            }
            return null;
        }

        public List<List<List<Field>>> GetFields()
        {
            return fields;
        }

        private bool IsValidCoordinate(int x, int y, int z)
        {
            return z >= 0 && z < fields.Count &&
                   y >= 0 && y < fields[z].Count &&
                   x >= 0 && x < fields[z][y].Count;
        }
    }
}
