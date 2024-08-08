using ConsoleAdventure.Content.Scripts;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine.Generate
{
    public class CaveGenerator
    {
        private World world;

        int walkerCount = 45;
        int descentCount = 5;

        int[] B = new int[] { 4, 6, 7, 8 };    //Отжиг
        int[] S = new int[] { 3, 5, 6, 7, 8 }; //
        int steps = 15;

        public void Generate(World world)
        {
            this.world = world;
            GenStone(0);
        }

        void GenStone(int w)
        {
            for(int i = 1; i < world.size - 1; i++)
            {
                for (int j = 1; j < world.size - 1; j++)
                {
                    new Floor(new(i, j), w);
                    new Stone(new(i, j), w);
                }
            }

            for (int i = 0; i < walkerCount; i++)
            {
                Vector2 diraction = new(((float)Generator.GenRand.Next(0, 100)) / 10f, ((float)Generator.GenRand.Next(0, 100)) / 10f);
                WorldGenUtils.RandomWalker(Generator.GenRand.Next(0, ConsoleAdventure.world.size), Generator.GenRand.Next(0, ConsoleAdventure.world.size), w, Generator.GenRand.Next(40, 101), diraction, MathHelper.ToDegrees(5), Generator.GenRand.Next(3, 7), 0, World.BlocksLayerId);
            }

            //постоброботка.

            bool[,] cells = WorldGenUtils.CellularAutomaton(WorldGenUtils.GetFieldCells(new(), new(ConsoleAdventure.world.size, ConsoleAdventure.world.size), 0, World.BlocksLayerId), steps, B, S);

            for (int i = 1; i < cells.GetLength(0) - 1; i++)
            {
                for (int j = 1; j < cells.GetLength(1) - 1; j++)
                {
                    if (cells[i, j]) 
                    {
                        new Stone(new(i, j), w); 
                    }

                    else
                    {
                        world.GetField(i, j, 1, w).content = null;
                    }
                }
            }

            for (int i = 1; i < world.size - 1; i++)
            {
                for (int j = 1; j < world.size - 1; j++)
                {
                    float noiceValue1 = OpenSimplex.noise2(ConsoleAdventure.world.seed, i * 0.05, j * 0.05);
                    float noiceValue2 = OpenSimplex.noise2(ConsoleAdventure.world.seed / 2, i * 0.05, j * 0.05);

                    bool hesField = ConsoleAdventure.world.GetField(i, j, World.BlocksLayerId, w)?.content != null;

                    if (noiceValue1 > 0.1f)
                    {
                        if (!(noiceValue2 > 0.3f))
                        {
                            if(hesField)
                                new Granite(new(i, j), w);

                            new GraniteFloor(new(i, j), w);
                        }
                    }

                    if(hesField && OpenSimplex.noise2(ConsoleAdventure.world.seed * 2, i * 0.05, j * 0.05) > 0.6)
                    {
                        new Quartz(new(i, j), w);
                    }
                }
            }

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
        }

            string cave = "";
            cave = world.LevelToString(w);         
        }
    }
}
