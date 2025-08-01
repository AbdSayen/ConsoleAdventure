using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine.Generate;
using ConsoleAdventure;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.WorldEngine.Generate
{
    [Serializable]
    public struct WalkerBuffer
    {
        public int minWalkers = 1;
        public int maxWalkers = 1;

        public int minSteps = 10;
        public int maxSteps = 10;

        public int minAngle = 90;
        public int maxAngle = 90;

        public int minSize = 2;
        public int maxSize = 2;

        public int minResizedSize = 2;
        public int maxResizedSize = 2;

        public int resizeChance = 100;

        /// <param name="walkers">Диапазон количества шагателей</param>
        /// <param name="steps">Диапазон количества шагов</param>
        /// <param name="angles">Диапазон максимальных уголов сдвига направления</param>
        /// <param name="sizes">Диапазон размеров шагателя</param>
        /// <param name="sizeLimits">Границы размера шагателя</param>
        /// <param name="resizeChance">Вероятность попытки изменить размер</param>
        public WalkerBuffer(Range walkers, Range steps, Range angles, Range sizes, Range sizeLimits, int resizeChance)
        {
            minWalkers = walkers.Start.Value;
            maxWalkers = walkers.End.Value;

            minSteps = steps.Start.Value;
            maxSteps = steps.End.Value;

            minAngle = angles.Start.Value;
            maxAngle = angles.End.Value;

            minSize = sizes.Start.Value;
            maxSize = sizes.End.Value;

            minResizedSize = sizeLimits.Start.Value;
            maxResizedSize = sizeLimits.End.Value;

            this.resizeChance = resizeChance;
        }

        public bool[,] GenerateMap(int mapWidth, int mapHaight, int seed, Position mapPos)
        {
            bool[,] map = new bool[mapWidth, mapHaight];

            int _x = (1 + mapPos.x) * mapWidth;
            int _y = (1 + mapPos.y) * mapHaight;

            for (int i = 0; i < WorldGenUtils.Adjust(OpenSimplex.noise2(seed, _x, _y), minWalkers, maxWalkers - 1); i++)
            {
                float value = OpenSimplex.noise2(seed + i, _x, _y);
                float value1 = OpenSimplex.noise2(seed + i + 1, _x, _y);
                float value2 = OpenSimplex.noise2(seed + i + 2, _x, _y);
                float value3 = OpenSimplex.noise2(seed + i + 3, _x, _y);

                int steps = WorldGenUtils.Adjust(value, minSteps, maxSteps);
                float angle = MathHelper.ToRadians(WorldGenUtils.Adjust(value, minAngle, maxAngle));

                int size = WorldGenUtils.Adjust(value, minSize, maxSize - 1);

                Vector2 direction = new Vector2(WorldGenUtils.Adjust(value, 0, 2) == 0 ? -1 : 1,
                WorldGenUtils.Adjust(value1, 0, 2) == 0 ? -1 : 1);

                Position position = new Position(WorldGenUtils.Adjust(value2, size, map.GetLength(0) - size),
                WorldGenUtils.Adjust(value3, size, map.GetLength(1) - size));

                map = WorldGenUtils.Walker(map, mapPos, seed, steps, angle, direction, position, size, minResizedSize, maxResizedSize, resizeChance);
            }

            return map;
        }  
    }
}
