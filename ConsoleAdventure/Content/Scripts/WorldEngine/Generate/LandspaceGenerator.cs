using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.WorldEngine;
using ConsoleAdventure.Content.Scripts.WorldEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAdventure.Content.Scripts;
using System.Windows.Forms.VisualStyles;

namespace ConsoleAdventure.WorldEngine.Generate
{
    public class LandspaceGenerator : EmptyGenerator
    {
        int seed;

        SineNoise climaticNoise;

        NoiseBuffer landspaceNoise;
        double landspaceScale;
        
        NoiseBuffer forestNoise;
        double forestScale;

        NoiseBuffer edgeNoise;
        double edgeScale;

        float seaLevel;
        float beachLevel;
        float edgeLevel;

        public override void LoadProperties(World world, Tags genProperties)
        {
            seed = world.seed;

            landspaceNoise = genProperties.SafelyGet<NoiseBuffer>("LandspaceNoise", new NoiseBuffer());
            landspaceScale = genProperties.SafelyGet<float>("LandspaceScale", 1);
            climaticNoise = genProperties.SafelyGet<SineNoise>("ClimaticNoise", new SineNoise());
            forestNoise = genProperties.SafelyGet<NoiseBuffer>("ForestNoise", new NoiseBuffer());
            forestScale = genProperties.SafelyGet<float>("ForestScale", 1); 
            edgeNoise = genProperties.SafelyGet<NoiseBuffer>("EdgeNoise", new NoiseBuffer());
            edgeScale = genProperties.SafelyGet<float>("EdgeScale", 1);

            seaLevel = genProperties.SafelyGet<float>("SeaLevel", 0);
            beachLevel = genProperties.SafelyGet<float>("BeachLevel", 0.1f);
            edgeLevel = genProperties.SafelyGet<float>("EdgeLevel", 0.1f);
        }

        public override async Task Generate(World world, Position startPosition, Position chunkPosition, Tags genProperties)
        {
            short X = startPosition.x;
            short Y = startPosition.y;

            for (int i = 0; i < Chunk.Size; i++)
            {
                for (int j = 0; j < Chunk.Size; j++)
                {
                    int x = X + i;
                    int y = Y + j;

                    /*for (int k = 0; k < 1000; k++)
                    {
                        new SandFloor(new Position(x, y), world.Surface);
                    }*/

                    await Place(world, x, y, chunkPosition);
                }
            }
        }

        public async Task Place(World world, int x, int y, Position chunkPosition)
        {
            float rawClimatic = climaticNoise.GetValue(chunkPosition.x, chunkPosition.y);

            Position position = new Position(x, y);

            float landspaceValue = landspaceNoise.FractalSimplex2(x * landspaceScale, y * landspaceScale);
            float climaticValue = rawClimatic + (landspaceValue / 4);

            if (landspaceValue >= seaLevel && (landspaceValue < beachLevel || climaticValue > 0.9f))
            {
                new SandFloor(position, world.Surface);
            }

            else if (landspaceValue >= beachLevel)
            {
                float treesValue = forestNoise.FractalSimplex2(x * forestScale, y * forestScale);
                float edgeValue = edgeNoise.FractalSimplex2(x * forestScale, y * edgeScale);

                if (edgeValue > edgeLevel)
                    treesValue -= edgeValue - edgeLevel;

                bool treeFlag = false;

                if (treesValue > 0)
                {
                    if ((((float)Utils.HashNoise(x, y, 100)) / 10) < treesValue)
                    {
                        new AppleTree(position, world.Surface);
                        treeFlag = true;
                    }
                }

                float noise2 = OpenSimplex.noise2(seed * 5, x * 0.6, y * 0.6);

                noise2 += OpenSimplex.noise2(seed - 5 * 3, x * 0.8, y * 0.8);
                noise2 -= treesValue;

                if (noise2 > 0.7 && !treeFlag)
                {
                    new Grass(position, world.Surface);
                }
            }

            else
            {
                new Water(position, world.Surface);
                new SandFloor(position, world.Sedimentary);
            }
        }
    }
}
