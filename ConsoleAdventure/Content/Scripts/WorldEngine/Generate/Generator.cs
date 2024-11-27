using ConsoleAdventure.Content.Scripts;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;


namespace ConsoleAdventure.WorldEngine.Generate
{
    public class Generator
    {
        private readonly int size;
        private readonly World world;

        private List<EmptyGenerator> pipeline = new List<EmptyGenerator>();

        public static Random GenRand { get; private set; }
        private static readonly object locker = new object();

        public Generator(World world, int size)
        {
            this.size = size;
            this.world = world;

            AddGeneratorToPipeline(new LandspaceGenerator());
            AddGeneratorToPipeline(new StructureGenerator());
            AddGeneratorToPipeline(new CaveGenerator());
        }

        public void AddGeneratorToPipeline(EmptyGenerator generator)
        {
            pipeline.Add(generator);
        }

        public async Task Generate(int seed, bool isfullGenerate = true)
        {
            GenRand = new Random(seed);
            ConsoleAdventure.world.seed = seed;

            await Generate(isfullGenerate);
        }

        public async Task Generate(bool isfullGenerate = true)
        {
            //lock (locker)
            //{
            //}

            world.InitializeChunks();

            // Generators
            GenerateBarriers();

            if (isfullGenerate)
            {
                for (int i = 0; i < pipeline.Count; i++)
                {
                    await pipeline[i].Generate(world);
                }
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
                            new Stone(new Position(x, y), w);
                        }
                    }
                }
            }
        }
    }
}