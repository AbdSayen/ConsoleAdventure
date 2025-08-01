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
using Microsoft.Xna.Framework;
using ConsoleAdventure.Content.Scripts.WorldEngine.Generate;

namespace ConsoleAdventure.WorldEngine.Generate
{
    public class CavernGenerator : EmptyGenerator
    {
        int seed;

        NoiseBuffer landspaceNoise;
        double landspaceScale;

        NoiseBuffer brownIronOreNoise;
        double brownIronOreScale;

        NoiseBuffer cavernNoise;
        double cavernScale;

        NoiseBuffer lavaCavernNoise;
        double lavaCavernScale;

        float seaLevel;
        float beachLevel;
        float plateBoundaryLevel;
        float brownIronOreLevel;

        float cavernLevel;
        float lavaCavernLevel;

        WalkerBuffer cavernTunnels;

        public override void LoadProperties(World world, Tags genProperties)
        {
            seed = world.seed;

            landspaceNoise = genProperties.SafelyGet<NoiseBuffer>("LandspaceNoise", new NoiseBuffer());
            landspaceScale = genProperties.SafelyGet<float>("LandspaceScale", 1);
            seaLevel = genProperties.SafelyGet<float>("SeaLevel", 0);
            beachLevel = genProperties.SafelyGet<float>("BeachLevel", 0.1f);
            plateBoundaryLevel = genProperties.SafelyGet<float>("PlateBoundary", -0.2f); 

            brownIronOreNoise = genProperties.SafelyGet<NoiseBuffer>("BrownIronOreNoise", new NoiseBuffer());
            brownIronOreScale = genProperties.SafelyGet<float>("BrownIronOreScale", 1f);
            brownIronOreLevel = genProperties.SafelyGet<float>("BrownIronOreLevel", 0f);

            cavernNoise = genProperties.SafelyGet<NoiseBuffer>("CavernNoise", new NoiseBuffer());
            cavernScale = genProperties.SafelyGet<float>("CavernScale", 1f);
            cavernLevel = genProperties.SafelyGet<float>("CavernLevel", 0f);

            lavaCavernNoise = genProperties.SafelyGet<NoiseBuffer>("LavaCavernNoise", new NoiseBuffer());
            lavaCavernScale = genProperties.SafelyGet<float>("LavaCavernScale", 1f);
            lavaCavernLevel = genProperties.SafelyGet<float>("LavaCavernLevel", 0f);

            cavernTunnels = genProperties.SafelyGet<WalkerBuffer>("CavernTunnels", new WalkerBuffer());
        }

        public override async Task Generate(World world, Position startPosition, Position chunkPosition, Tags genProperties)
        {
            short X = startPosition.x;
            short Y = startPosition.y;

            Position mapPos = chunkPosition / 3;
            int mapSize = Chunk.Size * 3;

            bool[,] cavernMap = cavernTunnels.GenerateMap(mapSize, mapSize, seed, mapPos);
            bool[,] lavaCavernMap = cavernTunnels.GenerateMap(mapSize, mapSize, seed + 1, mapPos);


            for (int i = 0; i < Chunk.Size; i++)
            {
                for (int j = 0; j < Chunk.Size; j++)
                {
                    int x = X + i;
                    int y = Y + j;

                    Position position = new Position(x, y);

                    float brownIronOreValue = brownIronOreNoise.FractalSimplex2(x * brownIronOreScale, y * brownIronOreScale);
                    float landSpaceValue = landspaceNoise.FractalSimplex2(x * landspaceScale, y * landspaceScale);
                    float cavernValue = cavernNoise.FractalSimplex2(x * cavernScale, y * cavernScale);
                    float lavaCavernValue = lavaCavernNoise.FractalSimplex2(x * lavaCavernScale, y * lavaCavernScale);

                    new Alfisol(position, world.Sedimentary);
                    new AlfisolFloor(position, world.Sedimentary);

                    if (brownIronOreValue > brownIronOreLevel)
                    {
                        new BrownIronOre(position, world.Sedimentary);
                    }

                    Position pos = ((chunkPosition - (mapPos * 3)) * Chunk.Size) + new Position(i, j);

                    if (cavernValue > cavernLevel && !cavernMap[pos.x, pos.y])
                    {
                        new Granite(position, world.Cavern);
                    }   

                    if (lavaCavernValue > lavaCavernLevel && !lavaCavernMap[pos.x, pos.y])
                    {
                        new Granulite(position, world.LavaCavern);
                    }

                    new GraniteFloor(position, world.Cavern);
                    new GranuliteFloor(position, world.LavaCavern);

                    if (landSpaceValue < plateBoundaryLevel)
                    {
                        new Basalt(position, world.Cavern);
                        new Basalt(position, world.LavaCavern);

                        new BasaltFloor(position, world.Cavern);
                        new BasaltFloor(position, world.LavaCavern);
                    }
                }
            }
        }
    }
}
