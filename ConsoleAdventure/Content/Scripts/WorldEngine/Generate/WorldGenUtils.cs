using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine.Generate;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine.Generate
{
    public static class WorldGenUtils
    {
        private static object[] Resizes = new object[] { -1, 0, 0, 1 };

        /// <summary>
        /// Шагатель по миру --устаревший метод, не рекомендуемый--.
        /// </summary>
        /// <param name="x">Координата по X</param>
        /// <param name="y">Координата по Y</param>
        /// <param name="w">Глубина</param>
        /// <param name="steps">Количество шагов</param>
        /// <param name="direction">Начальное направление валкера</param>
        /// <param name="offset">Максимальный угол сдвига направления</param>
        /// <param name="size">Стартовый размер валкера</param>
        /// <param name="type">Тип трансформа</param>
        /// <param name="layer">Слой трансформа</param>
        public static void LegacyWalker(int x, int y, int w, int steps, Vector2 direction, float offset, int size, int type, int layer = -1)
        {
            int curSize = size;
            Position position = new Position(x, y);
            Vector2 curDir = direction;

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

        /// <summary>
        /// Шагатель по карте прегенерации
        /// </summary>
        /// <param name="map">Карта прегенерации</param>
        /// <param name="mapPosition">Позиция карты</param>
        /// <param name="seed">Сид для рандома</param>
        /// <param name="steps">Количество шагов</param>
        /// <param name="angleOffset">Максимальный угол сдвига направления</param>
        /// <param name="direction">Начальное направление валкера</param>
        /// <param name="position">Начальная позиция</param>
        /// <param name="size">Начальная позиция</param>
        /// <param name="minSize">Минимальный размер валкера</param>
        /// <param name="maxSize">Максимальный размер валкера</param>
        /// <param name="chanceResize">Вероятность попытки изменить размер</param>
        /// <returns></returns>
        public static bool[,] Walker(bool[,] map, Position mapPosition, int seed, int steps, float angleOffset, Vector2 direction, Position position, int size, int minSize, int maxSize, int chanceResize = 100)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            Vector2 envoyPosition = position.ToVector2();
            Vector2 envoyDirection = direction;
            envoyDirection.Normalize();

            int envoySize = Math.Abs(size);

            for (int i = 0; i < steps; i++)
            {
                float x = envoyPosition.X;
                float y = envoyPosition.Y;

                for (int j = (int)x - envoySize; j <= (int)x + envoySize; j++)
                {
                    for (int k = (int)y - envoySize; k <= (int)y + envoySize; k++)
                    {
                        float SquareX = MathF.Pow(Math.Abs(j - x), 2);
                        float SquareY = MathF.Pow(Math.Abs(k - y), 2);

                        if (MathF.Sqrt(SquareX + SquareY) < (float)envoySize / 2.0f && j >= 0 && j < width && k >= 0 && k < height)
                        {
                            map[j, k] = true;
                        }
                    }
                }

                float value = OpenSimplex.noise2(seed, x + mapPosition.x, y + mapPosition.y);

                envoyPosition += envoyDirection;
                envoyDirection = Utils.Rotated(envoyDirection, value * angleOffset);
                envoyDirection.Normalize();

                int radius = (int)((float)envoySize / 2.0f);

                int randSize = radius + (int)value;

                if (randSize >= minSize && randSize < maxSize && (value + 1) * 50 < chanceResize)
                    envoySize = randSize;

                if (envoyPosition.X < radius || envoyPosition.X > width - radius || envoyPosition.Y < radius || envoyPosition.Y > height - radius)
                {
                    break;
                }
            }

            return map;
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

                        if (isAlive && S.Length > 0)
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
                        else if (B.Length > 0)
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

        /// <summary>
        /// Подгоняет значение из диапазона [-1, 1] до [<paramref name="min"/> , <paramref name="max"/>]. 
        /// Нужен для использования шума в качестве детерминированного рандома.
        /// </summary>
        /// <param name="value">Значения шума</param>
        /// <param name="min">минимальное значенения выхода</param>
        /// <param name="max">максимальное значенения выхода</param>
        public static float Adjust(float value, float min, float max)
        {
            if (value < -1) value = -1;
            if (value > 1) value = 1;

            float num = (value + 1f) / 2f;
            float difference = max - min;
            num = num * difference;
            return num + min;
        }

        /// <summary>
        /// Подгоняет значение из диапазона [-1, 1] до [<paramref name="min"/> , <paramref name="max"/>]. 
        /// Нужен для использования шума в качестве детерминированного рандома.
        /// </summary>
        /// <param name="value">Значения шума</param>
        /// <param name="min">минимальное значенения выхода</param>
        /// <param name="max">максимальное значенения выхода</param>
        public static int Adjust(float value, int min, int max)
        {
            if (value < -1) value = -1;
            if (value > 1) value = 1;

            float num = (value + 1f) / 2f;
            int difference = max - min;
            num = num * difference;
            return ((int)num) + min;
        }
    }
}
