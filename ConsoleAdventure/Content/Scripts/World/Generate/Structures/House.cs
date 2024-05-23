using ConsoleAdventure.Settings;
using ConsoleAdventure.World;
using ConsoleAdventure.WorldEngine;
using System;

namespace ConsoleAdventure.Generate.Structures
{
    internal class House : Structure
    {
        public static void Build(WorldEngine.World world, Position startPosition, int sizeX, int sizeY, Rotation rotation, Random random)
        {
            int doorPosition = 0;

            if (rotation == Rotation.up || rotation == Rotation.down)
            {
                doorPosition = startPosition.y + random.Next(1, sizeY - 1);
            }
            else if (rotation == Rotation.left || rotation == Rotation.right)
            {
                doorPosition = startPosition.x + random.Next(1, sizeX - 1);
            }

            for (int y = startPosition.y; y < startPosition.y + sizeY; y++)
            {
                for (int x = startPosition.x; x < startPosition.x + sizeX; x++)
                {
                    Field field = world.fields[WorldEngine.World.BlocksLayerId][y][x];
                    new Floor(world, new Position(x, y), WorldEngine.World.FloorLayerId);
                    if (x == startPosition.x || y == startPosition.y || x == startPosition.x + sizeX - 1 || y == startPosition.y + sizeY - 1)
                    {

                        if (x != doorPosition)
                        {
                            new Wall(world, new Position(x, y));
                            if(random.Next(0, 15) == 0)
                            {
                                new Ruine(world, new Position(x, y), WorldEngine.World.BlocksLayerId);
                            }
                        }
                        else
                        {
                            new Door(world, new Position(x, y), WorldEngine.World.BlocksLayerId);
                        }
                    }
                    field.structureName = $"House #{startPosition.x + startPosition.y}";
                    field.isStructure = true;
                }
            }
        }

    }
}
