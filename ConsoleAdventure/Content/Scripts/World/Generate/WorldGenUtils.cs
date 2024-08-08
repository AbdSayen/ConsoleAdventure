using ConsoleAdventure.WorldEngine.Generate;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine.Generate
{
    public static class WorldGenUtils
    {
        private static object[] Resizes = new object[] { -1, 0, 0, 1 };

        public static void RandomWalker(int x, int y, int w, int steps, Vector2 diraction, float offset, int size, int type, int layer = -1)
        {
            int curSize = size;
            Position position = new Position(x, y);
            Vector2 curDir = diraction;

            for(int i = 0; i < steps; i++)
            {
                curSize += (int)Generator.GenRand.Choose(Resizes);

                if(curSize < 1)
                {
                    curSize = 1;
                }

                curDir = curDir.Rotated(Generator.GenRand.Next((int)-offset, (int)offset));
                curDir = new(MathF.Min(curDir.X, curSize), MathF.Min(curDir.Y, curSize));

                Position start = position;
                position = start + curDir.ToPosition();//new Position((int)MathHelper.Lerp(position.x, start.x + curOffset.X, 0.1f), (int)MathHelper.Lerp(position.y, start.y + curOffset.Y, 0.1f));

                for(int j = 0; j < curSize; j++)
                {
                    for(int k = 0; k < curSize; k++)
                    {
                        Transform.SetObject(type, new(j + position.x, k + position.y), w, layer);
                        Transform.SetObject(type, new(j + ((position.x + start.x) / 2), k + ((position.y + start.y) / 2)), w, layer);
                    }
                }
            }
        }

        private static Position[] directions = new Position[]
        {
            new(-1, -1),
            new(0, -1),
            new(1, -1),
            new(1, 0),
            new(1, 1),
            new(0, 1),
            new(-1, 1),
            new(-1, 0)
        };

        public static bool[,] CellularAutomaton(bool[,] fieldCells, int steps, int[] B, int[] S)
        {
            int rows = fieldCells.GetLength(0);
            int cols = fieldCells.GetLength(1);

            for (int step = 0; step < steps; step++)
            {
                bool[,] newFieldCells = new bool[rows, cols];

                for (int j = 0; j < rows; j++)
                {
                    for (int k = 0; k < cols; k++)
                    {
                        int neighborsCount = 0;

                        foreach (var dir in directions)
                        {
                            int newRow = j + dir.x;
                            int newCol = k + dir.y;

                            if (newRow >= 0 && newRow < rows && newCol >= 0 && newCol < cols && fieldCells[newRow, newCol])
                            {
                                neighborsCount++;
                            }
                        }

                        bool isAlive = fieldCells[j, k];

                        if (isAlive)
                        {
                            bool survives = false;
                            for (int l = 0; l < S.Length; l++)
                            {
                                if (neighborsCount == S[l])
                                {
                                    survives = true;
                                    break;
                                }
                            }
                            newFieldCells[j, k] = survives;
                        }
                        else
                        {
                            for (int l = 0; l < B.Length; l++)
                            {
                                if (neighborsCount == B[l])
                                {
                                    newFieldCells[j, k] = true;
                                    break;
                                }
                            }
                        }
                    }
                }

                fieldCells = newFieldCells;
            }

            return fieldCells;
        }

        public static bool[,] GetFieldCells(Position start, Position end, int w, int layer, int[] typeFilter = null)
        {
            int width = end.x - start.x;
            int height = end.y - start.y;
            
            bool[,] fieldCells = new bool[width, height];

            for (int i = start.x; i < width; i++)
            {
                for (int j = start.y; j < height; j++)
                {
                    if (ConsoleAdventure.world.GetField(i, j, layer, w).content != null)
                    {
                        fieldCells[i, j] = true;
                    }
                }
            }

            return fieldCells;
        }
    }
}
