using CaModLoaderAPI;
using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Content.Scripts.WorldEngine.Generate;
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

        private Dictionary<int, List<WorldPropertiesGenerator>> propertiesPriorityMatrix = new Dictionary<int, List<WorldPropertiesGenerator>>();
        private List<WorldPropertiesGenerator> propertiesPipeline = new List<WorldPropertiesGenerator>();

        private Dictionary<int, List<EmptyGenerator>> generatorsPriorityMatrix = new Dictionary<int, List<EmptyGenerator>>();
        private List<EmptyGenerator> generationsPipeline = new List<EmptyGenerator>();

        public static Random GenRand { get; private set; }
        private static readonly object locker = new object();

        public Generator(World world, int size, bool isInitedContent = true, bool isGenerate = true)
        {
            this.size = size;
            this.world = world;

            if (isInitedContent)
            {
                Main.InitTransformsTypes(Main.vanillaTypesInitialized);
                WorldIO.InitContent();

                world.UnloadAllChunks();
            }

            if (CaModLoader.WorldGeneratorPreBuildPipelineMods(this))
            {
                AddGeneratorToPipeline(new LandspaceGenerator(), 100);
            }

            ConvertPrioritiesToGenPipeline();

            CaModLoader.WorldGeneratorBuildPipelineMods(this);

            if (isGenerate)
            {
                if (CaModLoader.WorldPropertiesPreBuildPipelineMods(this))
                {
                    AddPropertiesToPipeline(new NoisesProperties(), 100);
                }

                CaModLoader.WorldPropertiesBuildPipelineMods(this);
            }
        }

        public void AddGeneratorToPipeline(EmptyGenerator generator, int order = -1)
        {
            if (!generatorsPriorityMatrix.ContainsKey(order))
                generatorsPriorityMatrix.Add(order, new List<EmptyGenerator> { generator });
            else
                generatorsPriorityMatrix[order].Add(generator);
        }

        public void AddPropertiesToPipeline(WorldPropertiesGenerator generator, int order = -1)
        {
            if (!propertiesPriorityMatrix.ContainsKey(order))
                propertiesPriorityMatrix.Add(order, new List<WorldPropertiesGenerator> { generator });
            else
                propertiesPriorityMatrix[order].Add(generator);
        }


        private void ConvertPrioritiesToGenPipeline()
        {
            SortedDictionary<int, List<EmptyGenerator>> srtd = new SortedDictionary<int, List<EmptyGenerator>>(generatorsPriorityMatrix);
            List<int> keys = srtd.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                List<EmptyGenerator> v = srtd[keys[i]];
                for (int j = 0; j < v.Count; j++)
                {
                    generationsPipeline.Add(v[j]);
                }
            }
        }

        private void ConvertPrioritiesToPropPipeline()
        {
            SortedDictionary<int, List<WorldPropertiesGenerator>> srtd = new SortedDictionary<int, List<WorldPropertiesGenerator>>(propertiesPriorityMatrix);
            List<int> keys = srtd.Keys.ToList();
            for (int i = 0; i < keys.Count; i++)
            {
                List<WorldPropertiesGenerator> v = srtd[keys[i]];
                for (int j = 0; j < v.Count; j++)
                {
                    propertiesPipeline.Add(v[j]);
                }
            }
        }

        public async Task CreateWorld(int seed, bool isfullGenerate = true)
        {
            try
            {
                ConvertPrioritiesToPropPipeline();

                GenRand = new Random(seed);
                ConsoleAdventure.world.seed = seed;

                await CreateProperties(isfullGenerate);
            }

            catch (Exception ex)
            {
                string error = $"{Localization.GetTranslation("UI", "WorldLoadError")}\n\n{ex.GetType()}: {ex.Message}\n{ex.InnerException}\n{ex.StackTrace}\n{ex.Source}\n{ex.TargetSite}";
                ConsoleAdventure.menu.OpenWorldError(error);
                ConsoleAdventure.logger.AddException(error);
            }
        }

        private async Task CreateProperties(bool isFullGenerate = true)
        {
            world.InitializeChunks();

            if (isFullGenerate)
            {
                for (int i = 0; i < propertiesPipeline.Count; i++)
                {
                    await propertiesPipeline[i].UpdateWorldProperties(world.generationProperties, world);
                }
            }
        }

        public async Task Generate(World world, Position chunkPos)
        {
            for (int i = 0; i < generationsPipeline.Count; i++)
            {
                await generationsPipeline[i].Generate(world, chunkPos * Chunk.Size, chunkPos, world.generationProperties);
            }
        }
    }
}