using System.Collections.Generic;

namespace ConsoleAdventure.World
{
    internal class Generator
    {
        private int size;
        private List<List<List<Field>>> fields;
        public Generator(List<List<List<Field>>> fields, int size) 
        {
            this.size = size;
            this.fields = fields;
        }

        public void Generate()
        {
            GenerateSpace();
            GenerateBarriers();
        }

        private void GenerateSpace()
        {
            for (int z = 0; z < World.CountOfLayers; z++)
            {
                fields.Add(new List<List<Field>>());
                for (int y = 0; y < size; y++)
                {
                    fields[z].Add(new List<Field>());
                    for (int x = 0; x < size; x++)
                    {
                        fields[z][y].Add(new Field());
                    }
                }
            }
        }
        
        private void GenerateBarriers()
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (y == size - 1 || y == 0 || x == size - 1 || x == 0)
                    {
                        Field field = fields[World.BlocksLayerId][x][y];
                        field.type = FieldType.wall;
                        if (field.content != null)
                        {
                            field.content.x = x;
                            field.content.y = y;
                        }
                    }
                }
            }
        }

        //private void GenerateObstacles()
        //{
        //    for(int y = 0; y < fields[World.BlocksLayerId].Count; y++)
        //    {
        //        for (int x = 0; x < fields[World.BlocksLayerId][y].Count; x++)
        //        {
        //              
        //        }
        //    }
        //}
    }
}
