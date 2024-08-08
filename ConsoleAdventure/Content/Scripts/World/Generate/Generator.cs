using ConsoleAdventure.Content.Scripts;
using System;


namespace ConsoleAdventure.WorldEngine.Generate
{
    public class Generator
    {
        private readonly int size;
        private readonly World world;

        private LandspaceGenerator landspaceGenerator = new LandspaceGenerator();
        private StructureGenerator structureGenerator = new StructureGenerator();
        private CaveGenerator caveGenerator = new CaveGenerator();

        public static Random GenRand { get; private set; }

        public Generator(World world, int size)
        {
            this.size = size;
            this.world = world;
        }

        public void Generate(int seed, bool isfullGenerate = true)
        {
            GenRand = new Random(seed);
            ConsoleAdventure.world.seed = seed;
            Generate(isfullGenerate);

            ConsoleAdventure.world.entities.Add(new Cat(new(4, 4), ConsoleAdventure.StartDeep));
        }


        public void Generate(bool isfullGenerate = true)
        {
            world.InitializeChunks();
            GenerateBarriers();

            if(isfullGenerate)
            {
                structureGenerator.Generate(world);
                landspaceGenerator.Generate(world);
                caveGenerator.Generate(world);
            }
        }

        private void GenerateBarriers()
        {
            for (int w = 0; w < Chunk.maxDeep; w++)
            {
                for (int y = 0; y < size; y++)
                {
                    for (int x = 0; x < size; x++)
                    {
                        if (y == size - 1 || y == 0 || x == size - 1 || x == 0)
                        {
                            //Field field = world.GetField(x, y, World.BlocksLayerId, ConsoleAdventure.StartDeep);
                            new Tree(new Position(x, y), w);
                        }
                    }
                }
            }
        }
    }
}