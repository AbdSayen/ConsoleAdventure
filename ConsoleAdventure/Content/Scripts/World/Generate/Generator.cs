using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine.Generate
{
    public class Generator
    {
        private Random random;
        private readonly int size;
        private readonly World world;

        private LandspaceGenerator landspaceGenerator = new LandspaceGenerator();
        private StructureGenerator structureGenerator = new StructureGenerator();

        public Generator(World world, int size)
        {
            this.size = size;
            this.world = world;
        }

        public void Generate(int seed)
        {
            random = new Random(seed);
            Generate();
        }

        public void Generate()
        {
            random = new Random();
            GenerateSpace();
            GenerateBarriers();
            structureGenerator.Generate(world, random);
            landspaceGenerator.Generate(world, random);
        }

        private void GenerateSpace()
        {
            for (int z = 0; z < World.CountOfLayers; z++)
            {
                world.fields.Add(new List<List<Field>>());
                for (int y = 0; y < size; y++)
                {
                    world.fields[z].Add(new List<Field>());
                    for (int x = 0; x < size; x++)
                    {
                        world.fields[z][y].Add(new Field());
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
                        Field field = world.fields[World.BlocksLayerId][x][y];
                        new Wall(world, new Position(x, y));
                    }
                }
            }
        }
    }
}