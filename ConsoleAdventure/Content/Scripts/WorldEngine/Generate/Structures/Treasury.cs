using ConsoleAdventure.WorldEngine.Generate;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleAdventure.Generate.Structures
{
    public class Treasury : Structure
    {
        public Treasury(Position position, int w, Random random, Point size, int rectanglesCount)
        {
            bool[,] aria = new bool[size.X, size.Y];

            for (int i = 0; i < rectanglesCount; i++)
            {
                int randX = random.Next(0, size.X / 2);
                int randY = random.Next(0, size.Y / 2);
                int width = aria.GetLength(0) / 2;
                int height = aria.GetLength(1) / 2;

                for (int j = 0; j < width; j++)
                {
                    for (int k = 0; k < height; k++)
                    {
                        aria[randX + j, randY + k] = true;
                    }
                }
            }

            for (int i = 0; i < aria.GetLength(0); i++)
            {
                for (int j = 0; j < aria.GetLength(1); j++)
                {
                    if (aria[i, j])
                    {
                        //Transform t = ConsoleAdventure.world.GetField(position.x + i, position.y + j, World.BlocksLayerId, w).content;
                        //if (t?.type != (int)RenderFieldType.descent && t?.type != (int)RenderFieldType.climb)
                        //{
                            ConsoleAdventure.world.GetField(position.x + i, position.y + j, World.BlocksLayerId, w).content = null;
                        //}
                        
                        ConsoleAdventure.world.GetField(position.x + i, position.y + j, World.FloorLayerId, w).isStructure = true;
                    }
                }
            }

            aria = WorldGenUtils.CellularAutomaton(aria, 1, new int[0], new int[] { 1, 2, 3, 4, 5, 6, 7 });

            for (int i = 0; i < aria.GetLength(0); i++)
            {
                for (int j = 0; j < aria.GetLength(1); j++)
                {
                    if (aria[i, j])
                    {
                        int randType = random.Next(0, 4);

                        if (randType == 0)
                        {
                            new Wall(new Position(position.x + i, position.y + j), w);
                        }

                        if (randType == 1)
                        {
                            new Ruine(new Position(position.x + i, position.y + j), w);
                        }
                    }
                }
            }

            int count = random.Next(2, 5);
            for (int i = 0; i < count; i++)
            {
                Position logPos;
                while (true)
                {
                    logPos = new Position(random.Next(0, aria.GetLength(0)), random.Next(0, aria.GetLength(1)));
                    Field field = ConsoleAdventure.world.GetField(position.x + logPos.x, position.y + logPos.y, World.FloorLayerId, w);
                    Field field1 = ConsoleAdventure.world.GetField(position.x + logPos.x, position.y + logPos.y, World.BlocksLayerId, w);

                    if (field?.isStructure == true && field1?.content == null)
                    {
                        break;
                    }
                }

                if(i == 0)
                {
                    List<Stack> items = new List<Stack>();

                    int itemsCount = random.Next(2, 6);
                    List<Stack> allElements = new List<Stack>()
                    {
                        new Stack(new BombItem(), random.Next(4, 10)),
                        new Stack(new TorchItem(), random.Next(5, 23)),
                        new Stack(new IronBar(), random.Next(1, 4)),
                        new Stack(new Log(), random.Next(7, 15)),
                        new Stack(new StoneItem(), random.Next(1, 5)),
                        new Stack(new WebItem(), random.Next(10, 17)),
                        new Stack(new BrownIronOreItem(), random.Next(3, 7)),
                    };

                    for (int j = allElements.Count - 1; j > 0; j--)
                    {
                        int n = random.Next(0, j + 1);
                        Stack temp = allElements[j];
                        allElements[j] = allElements[n];
                        allElements[n] = temp;
                    }

                    items = allElements.GetRange(0, itemsCount);

                    new Chest(position + logPos, w, items);
                }

                else
                {
                    new Plank(position + logPos, w);
                }
            }

            for (int i = 0; i < aria.GetLength(0); i++)
            {
                for (int j = 0; j < aria.GetLength(1); j++)
                {
                    if (ConsoleAdventure.world.GetField(position.x + i, position.y + j, World.FloorLayerId, w)?.isStructure == true)
                    {
                        if (ConsoleAdventure.world.GetField(position.x + i, position.y + j, World.BlocksLayerId, w)?.content == null && ConsoleAdventure.world.GetField(position.x + i, position.y + j, World.ItemsLayerId, w)?.content == null && random.Next(0, 3) == 0)
                        {
                            new Web(new (position.x + i, position.y + j), w);
                        }
                    }
                }
            }
        }
    }
}
