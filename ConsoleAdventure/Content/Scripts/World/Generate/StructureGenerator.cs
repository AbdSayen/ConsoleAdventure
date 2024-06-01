using ConsoleAdventure.Generate.Structures;
using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine.Generate
{
    public class StructureGenerator
    {
        private World world;
        private Random random;

        private int minHouseSizeY = 10;
        private int maxHouseSizeY = 25;
        private int minHouseSizeX = 10;
        private int maxHouseSizeX = 25;

        public void Generate(World world, Random random) 
        {
            this.world = world;
            this.random = random;

            GenerateHouses();
        }

        private void GenerateHouses()
        {
            List<List<Field>> fields = world.fields[World.BlocksLayerId];

            for (int y = 0; y < fields.Count; y++)
            {
                for (int x = 0; x < fields[y].Count; x++)
                {
                    int sizeX = random.Next(minHouseSizeX, maxHouseSizeX + 1);
                    int sizeY = random.Next(minHouseSizeY, maxHouseSizeY + 1);

                    if (random.Next(0, 1000) == 0 && CheckGeneratePossibility(new Position(x, y), sizeX, sizeY))
                    {
                        House.Build(world, new Position(x, y), sizeX, sizeY, Rotation.left, random);
                    }
                }
            }
        }

        private bool CheckGeneratePossibility(Position startPosition, int sizeX, int sizeY)
        {
            var layer = world.fields[WorldEngine.World.BlocksLayerId];
            int layerHeight = layer.Count;
            int layerWidth = layer[0].Count;

            for (int y = startPosition.y; y < startPosition.y + sizeY; y++)
            {
                if (y >= layerHeight || y < 0)
                {
                    return false;
                }
                for (int x = startPosition.x; x < startPosition.x + sizeX; x++)
                {
                    if (x >= layerWidth || x < 0 || layer[y][x] == null || layer[y][x].isStructure)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

    }
}
