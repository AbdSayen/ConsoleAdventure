using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.WorldEngine;
using ConsoleAdventure.Content.Scripts.WorldEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ConsoleAdventure.Content.Scripts;

namespace ConsoleAdventure.WorldEngine.Generate
{
    public class LandspaceGenerator : EmptyGenerator
    {
        public override async Task Generate(World world, Position startPosition, Position chunkPosition, Tags genProperties)
        {
            int seed = world.seed;

            NoiseBuffer landspaceNoise = genProperties.SafelyGet<NoiseBuffer>("LandspaceNoise", new NoiseBuffer());
            double landspaceScale = genProperties.SafelyGet<float>("LandspaceScale", 1);
            float seaLevel = genProperties.SafelyGet<float>("SeaLevel", 0);

            short X = startPosition.x;
            short Y = startPosition.y;

            NoiseBuffer trees = new NoiseBuffer(seed, 4, 0.5, 2);

            for (int i = 0; i < Chunk.Size; i++)
            {
                for (int j = 0; j < Chunk.Size; j++)
                {
                    float landspaceValue = landspaceNoise.FractalSimplex2((X + i) * landspaceScale, (Y + j) * landspaceScale);

                    if (landspaceValue >= seaLevel) 
                    {
                        float treesValue = trees.FractalSimplex2((X + i) * 0.01, (Y + j) * 0.01);

                        bool treeFlag = false;

                        if (treesValue > 0 && (((float)Utils.HashNoise(X + i, Y + j, 45)) / 10) < treesValue)
                        {
                            new Tree(new(X + i, Y + j), world.Surface);
                            treeFlag = true;
                        }

                        float noise2 = OpenSimplex.noise2(seed * 5, (X + i) * 0.6, (Y + j) * 0.6);
                        noise2 += OpenSimplex.noise2(seed - 5 * 3, (X + i) * 0.8, (Y + j) * 0.8);
                        noise2 -= treesValue;

                        if (noise2 > 0.7 && !treeFlag)
                        {
                            new Grass(new(X + i, Y + j), world.Surface);
                        }
                    }
                    else
                    {
                        new Water(new(X + i, Y + j), world.Surface);
                    }
                }
            }
        }
    }
}
