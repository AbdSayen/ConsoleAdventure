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
            //random = new Random();
            world.InitializeChunks();
            GenerateBarriers();
            //structureGenerator.Generate(world, random);
            //landspaceGenerator.Generate(world, random);
        }

        private void GenerateBarriers()
        {
            for (int y = 0; y < size; y++)
            {
                for (int x = 0; x < size; x++)
                {
                    if (y == size - 1 || y == 0 || x == size - 1 || x == 0)
                    {
                        Field field = world.GetField(x, y, World.BlocksLayerId);
                        new Tree(world, new Position(x, y));
                    }
                }
            }
        }
    }
}