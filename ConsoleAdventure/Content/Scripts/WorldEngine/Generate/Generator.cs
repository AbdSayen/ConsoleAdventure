using ConsoleAdventure.Content.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Threading;
using System.Threading.Tasks;


namespace ConsoleAdventure.WorldEngine.Generate
{
    public class Generator
    {
        private readonly int size;
        private readonly World world;

        private Dictionary<int, List<EmptyGenerator>> generatorsPriorityMatrix = new Dictionary<int, List<EmptyGenerator>>();
        private List<EmptyGenerator> pipeline = new List<EmptyGenerator>();

        public static Random GenRand { get; private set; }
        private static readonly object locker = new object();

        public Generator(World world, int size)
        {
            this.size = size;
            this.world = world;

            if (CaModLoader.WorldGeneratorPreBuildPipelineMods(this) && false)
            {
                AddGeneratorToPipeline(new StructureGenerator(), 100);
                AddGeneratorToPipeline(new LandspaceGenerator(), 200);
                AddGeneratorToPipeline(new CaveGenerator(), 300);
            }

            CaModLoader.WorldGeneratorBuildPipelineMods(this);
        }

        public void AddGeneratorToPipeline(EmptyGenerator generator, int order = -1)
        {
            if (!generatorsPriorityMatrix.ContainsKey(order))
                generatorsPriorityMatrix.Add(order, new List<EmptyGenerator> { generator });
            else
                generatorsPriorityMatrix[order].Add(generator);
        }

        private void ConvertPrioritiesToPipeline()
        {
            SortedDictionary<int, List<EmptyGenerator>> srtd = new SortedDictionary<int, List<EmptyGenerator>>(generatorsPriorityMatrix);
            List<int> keys = srtd.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                List<EmptyGenerator> v = srtd[keys[i]];
                for (int j = 0; j < v.Count; j++)
                {
                    pipeline.Add(v[j]);
                }
            }
        }

        public async Task Generate(int seed, bool isfullGenerate = true)
        {
            ConvertPrioritiesToPipeline();

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