using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Generate.Structures;
using Microsoft.Xna.Framework;
using System;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine.Generate
{
    public class CaveGenerator : EmptyGenerator
    {
        int walkerCount = 45;
        int descentCount = 5;

        int[] B = new int[] { 4, 6, 7, 8 };    //Отжиг
        int[] S = new int[] { 3, 5, 6, 7, 8 }; //
        int steps = 15;

        public override async Task Generate(World world)
        {
            await base.Generate(world);

            processHint = "Digging Caves...";

            GenStone(0);
        }

        void GenStone(int w)
        {
            processHint = "Digging Caves... [Filling stones]";
            iterateWorldChunkByChunk((int i, int j) => //    <- Вот пример чанковой генерации через весь мир
            {
                new Floor(new(i, j), w);
                new Stone(new(i, j), w);
            });

            processHint = "Digging Caves... [Walk trough stone]";
            for (int i = 0; i < walkerCount; i++)
            {
                Vector2 diraction = new(((float)Generator.GenRand.Next(0, 100)) / 10f, ((float)Generator.GenRand.Next(0, 100)) / 10f);
                WorldGenUtils.RandomWalker(Generator.GenRand.Next(0, ConsoleAdventure.world.size), Generator.GenRand.Next(0, ConsoleAdventure.world.size), w, Generator.GenRand.Next(40, 101), diraction, MathHelper.ToDegrees(5), Generator.GenRand.Next(3, 7), 0, World.BlocksLayerId);

                processProgress = (int)((float)i / walkerCount * 100);
            }

            //постоброботка.

            bool[,] cells = WorldGenUtils.CellularAutomaton(WorldGenUtils.GetFieldCells(new(), new(ConsoleAdventure.world.size, ConsoleAdventure.world.size), 0, World.BlocksLayerId), steps, B, S);

            processHint = "Digging Caves... [Randomizing]";
            iterateWorldChunkByChunk((int i, int j) => //    <- Вот пример чанковой генерации через весь мир
            {
                if (cells[i, j])
                {
                    new Stone(new(i, j), w);
                }

                else
                {
                    world.GetField(i, j, 1, w).content = null;
                }
            });

            processHint = "Digging Caves... [Adding some materials]";
            iterateWorldChunkByChunk((int i, int j) => //    <- Вот пример чанковой генерации через весь мир
            {
                float noiceValue1 = OpenSimplex.noise2(ConsoleAdventure.world.seed, i * 0.05, j * 0.05);
                float noiceValue2 = OpenSimplex.noise2(ConsoleAdventure.world.seed / 2, i * 0.05, j * 0.05);

                bool hesField = ConsoleAdventure.world.GetField(i, j, World.BlocksLayerId, w)?.content != null;

                if (noiceValue1 > 0.1f)
                {
                    if (!(noiceValue2 > 0.3f))
                    {
                        if (hesField)
                            new Granite(new(i, j), w);

                        new GraniteFloor(new(i, j), w);
                    }
                }

                if (hesField && OpenSimplex.noise2(ConsoleAdventure.world.seed * 2, i * 0.05, j * 0.05) > 0.6)
                {
                    new Quartz(new(i, j), w);
                }
            });

            processHint = "Digging Caves... [Adding holes]";
            for (int i = 0; i < descentCount; i++)
            {
                Position descentPos = new();
                while (true)
                {
                    descentPos = new(Generator.GenRand.Next(1, world.size - 1), Generator.GenRand.Next(1, world.size - 1));

                    if (world.GetField(descentPos.x, descentPos.y, World.BlocksLayerId, w)?.content == null) 
                    {
                        if (world.GetField(descentPos.x, descentPos.y, World.BlocksLayerId, w + 1)?.content == null)
                        {
                            break;
                        }
                    }
                }

                new Climb(descentPos, w);
                new Descent(descentPos, w + 1);

                processProgress = (int)((float)i / descentCount * 100);
            }

            processHint = "Digging Caves... [Adding Brown Iron]";
            for (int i = 0; i < 70; i++)
            {
                Position position = new(Generator.GenRand.Next(1, world.size - 1), Generator.GenRand.Next(1, world.size - 1));

                int width = Generator.GenRand.Next(8, 16);
                int height = Generator.GenRand.Next(8, 16);

                bool[,] oreArea = new bool[width, height];

                int[] BOre = new int[] { 3, 5, 6, 7, 8 }; 
                int[] SOre = new int[] { 5, 6, 7, 8 }; 

                for (int j = 0; j < width; j++)
                {
                    for (int k = 0; k < height; k++)
                    {
                        oreArea[j, k] = Generator.GenRand.Next(0, 4) > 1;
                    }
                }

                oreArea = WorldGenUtils.CellularAutomaton(oreArea, 6, BOre, SOre);

                for (int j = 0; j < width; j++)
                {
                    for (int k = 0; k < height; k++)
                    {
                        if (oreArea[j, k] && world.GetField(position.x + j, position.y + k, World.BlocksLayerId, w)?.content != null)
                        {
                            new BrownIronOre(new(j + position.x, k + position.y), w);
                        }
                    }
                }

                processProgress = (int)((float)i / 70 * 100);
            }

            processHint = "Digging Caves... [Adding Water]";
            iterateWorldChunkByChunk((int i, int j) => //    <- Вот пример чанковой генерации через весь мир
            {
                float noiceValue1 = OpenSimplex.noise2(ConsoleAdventure.world.seed * w * 4, i * 0.05, j * 0.05);
                if (noiceValue1 < -0.6f && ConsoleAdventure.world.GetField(i, j, World.BlocksLayerId, w)?.content == null)
                {
                    new Water(new(i, j), w);
                }
            });

            processHint = "Digging Caves... [Adding Stalactites and Treasures]";
            for (int i = 0; i < 21; i++)
            {
                Position stalactitePos = new(Generator.GenRand.Next(1, world.size - 1), Generator.GenRand.Next(1, world.size - 1));

                for (int j = 0; j < 3; j++)
                {
                    GenerateStalactitesArea(stalactitePos + new Position(Generator.GenRand.Next(-15, 15), Generator.GenRand.Next(-15, 15)), Generator.GenRand.Next(5, 25), Generator.GenRand.Next(5, 25), w);
                }

                processProgress = (int)((float)i / 21 * 100);
            }

            //for (int i = 0; i < 8; i++)
            //{
            //    new Treasury(new Position(Generator.GenRand.Next(0, world.size), Generator.GenRand.Next(0, world.size)), w, Generator.GenRand, new Point(Generator.GenRand.Next(10, 15), Generator.GenRand.Next(10, 15)), 4);
            //}

            //string cave = "";
            //cave = world.LevelToString(w);         
        }

        private void GenerateStalactitesArea(Position position, int width, int height, int w)
        {
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    Position curPos = position + new Position(i, j);
                    if (Generator.GenRand.Next(0, 5) > 3 && ConsoleAdventure.world.GetField(curPos.x, curPos.y, World.BlocksLayerId, w)?.content == null)
                    {
                        new Stalactite(curPos, w);
                    }

                    processProgress = (int)((float)(i * width + j) / (width * height) * 100);
                }
            }
        }
    }
}
