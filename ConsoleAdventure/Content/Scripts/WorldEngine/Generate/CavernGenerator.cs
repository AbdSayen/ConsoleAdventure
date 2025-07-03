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

namespace ConsoleAdventure.WorldEngine.Generate
{
    public class CavernGenerator : EmptyGenerator
    {
        int seed;

        NoiseBuffer landspaceNoise;
        double landspaceScale;

        NoiseBuffer brownIronOreNoise;
        double brownIronOreScale;

        float seaLevel;
        float beachLevel;
        float brownIronOreLevel;

        bool[,] map = new bool[Chunk.Size * 3, Chunk.Size * 3];

        public override void LoadProperties(World world, Tags genProperties)
        {
            seed = world.seed;

            landspaceNoise = genProperties.SafelyGet<NoiseBuffer>("LandspaceNoise", new NoiseBuffer());
            landspaceScale = genProperties.SafelyGet<float>("LandspaceScale", 1);

            seaLevel = genProperties.SafelyGet<float>("SeaLevel", 0);
            beachLevel = genProperties.SafelyGet<float>("BeachLevel", 0.1f);

            brownIronOreNoise = genProperties.SafelyGet<NoiseBuffer>("BrownIronOreNoise", new NoiseBuffer());
            brownIronOreScale = genProperties.SafelyGet<float>("BrownIronOreScale", 1f);
            brownIronOreLevel = genProperties.SafelyGet<float>("BrownIronOreLevel", 0f);
        }

        public override async Task Generate(World world, Position startPosition, Position chunkPosition, Tags genProperties)
        {
            short X = startPosition.x;
            short Y = startPosition.y;

            //bool[,] map = new bool[Chunk.Size * 3, Chunk.Size * 3];

            //for (int i = 0; i < map.GetLength(0); i++)
            //{
            //    for (int j = 0; j < map.GetLength(1); j++)
            //    {
            //        map[i, j] = false;
            //    }
            //}

            /*int minWalkers = 1;
            int maxWalkers = 10;

            int minSteps = 15;
            int maxSteps = 46;

            int minAngle = 20;
            int maxAngle = 45;

            int minSize = 2;
            int maxSize = 4;

            int minResizedSize = 2;
            int maxResizedSize = 15;

            int resizeChance = 100;
            */
            Position mapPos = chunkPosition / 3;
            /*
            int _x = (1 + mapPos.x) * (16 * 3);
            int _y = (1 + mapPos.y) * (16 * 3);

            for (int i = 0; i < Adjust(OpenSimplex.noise2(seed, _x, _y), minWalkers, maxWalkers - 1); i++)
            {
                float value = OpenSimplex.noise2(seed + i, _x, _y);
                float value1 = OpenSimplex.noise2(seed + i + 1, _x, _y);
                float value2 = OpenSimplex.noise2(seed + i + 2, _x, _y);
                float value3 = OpenSimplex.noise2(seed + i + 3, _x, _y);

                int steps = Adjust(value, minSteps, maxSteps);
                float angle = MathHelper.ToRadians(Adjust(value, minAngle, maxAngle));

                int size = Adjust(value, minSize, maxSize - 1);

                Vector2 direction = new Vector2(Adjust(value, 0, 2) == 0 ? -1 : 1,
                                                Adjust(value1, 0, 2) == 0 ? -1 : 1);

                Position position = new Position(Adjust(value2, size, map.GetLength(0) - size),
                                           Adjust(value3, size, map.GetLength(1) - size));

                map = WorldGenUtils.Walker(map, mapPos, seed, steps, angle, direction, position, size, minResizedSize, maxResizedSize, resizeChance);
            }*/

            for (int i = 0; i < Chunk.Size; i++)
            {
                for (int j = 0; j < Chunk.Size; j++)
                {
                    int x = X + i;
                    int y = Y + j;

                    Position position = new Position(x, y);

                    float brownIronOreValue = brownIronOreNoise.FractalSimplex2(x * brownIronOreScale, y * brownIronOreScale);

                    new Alfisol(position, world.Sedimentary);
                    new AlfisolFloor(position, world.Sedimentary);

                    if (brownIronOreValue > brownIronOreLevel)
                    {
                        new BrownIronOre(position, world.Sedimentary);
                    }

                    Position pos = (mapPos - chunkPosition);
                    if (brownIronOreValue < 0.1)
                    {
                        new Granite(position, world.Cavern);
                        new Granite(position, world.LavaCavern);
                    }
                    
                    new GraniteFloor(position, world.Cavern);
                    new GraniteFloor(position, world.LavaCavern);
                }
            }
        }

        public int Adjust(float value, int min, int max)
        {
            if (value < -1) value = -1;
            if (value > 1) value = 1;

            float num = (value + 1f) / 2f;
            int difference = max - min;
            num = num * difference;
            return ((int)num) + min;
        }

        public float Adjust(float value, float min, float max)
        {
            if (value < -1) value = -1;
            if (value > 1) value = 1;

            float num = (value + 1f) / 2f;
            float difference = max - min;
            num = num * difference;
            return num + min;
        }
    }
}
