using ConsoleAdventure.WorldEngine;
using System;
using ConsoleAdventure.Content.Scripts.Abstracts;

namespace ConsoleAdventure.Generate.Structures
{
    public class House : Structure
    {
        public static void Build(World world, Position startPosition, int sizeX, int sizeY, Rotation rotation, Random random)
        {
            for (int y = startPosition.y; y < startPosition.y + sizeY; y++)
            {
                for (int x = startPosition.x; x < startPosition.x + sizeX; x++)
                {
                    Field field = world.GetField(x, y, World.BlocksLayerId);
                    if (field == null)
                    {
                        field = new Field();
                        world.SetField(x, y, World.BlocksLayerId, field);
                    }

                    new Floor(world, new Position(x, y), World.FloorLayerId);
                    if (x == startPosition.x || y == startPosition.y || x == startPosition.x + sizeX - 1 || y == startPosition.y + sizeY - 1)
                    {

                        if (rotation == Rotation.left || rotation == Rotation.right)
                        {
                            new Wall(world, new Position(x, y));
                            if (random.Next(0, 15) == 0)
                            {
                                new Ruine(world, new Position(x, y), World.BlocksLayerId);
                            }
                        }

                        else
                        {
                            new Wall(world, new Position(x, y));
                            if (random.Next(0, 15) == 0)
                            {
                                new Ruine(world, new Position(x, y), World.BlocksLayerId);
                            }
                        }

                    }
                    field.structureName = $"House #{startPosition.x + startPosition.y}";
                    field.isStructure = true;
                }
            }

            int doorWallID1 = random.Next(0, 4);
            int doorWallID2 = 0;

            while (true) //Исключение одинакового результата
            {
                if (doorWallID1 == doorWallID2)
                {
                    doorWallID2 = random.Next(0, 4);
                }

                else break;
            }

            Position[] positions = {
                                    new Position(0, random.Next(1, sizeY - 1)),
                                    new Position(sizeX - 1, random.Next(1, sizeY - 1)),
                                    new Position(random.Next(1, sizeX - 1), 0),
                                    new Position(random.Next(1, sizeX - 1), sizeY - 1)
                                   };

            new Door(world, startPosition + positions[doorWallID1], World.BlocksLayerId);
            new Door(world, startPosition + positions[doorWallID2], World.BlocksLayerId);
        }

        /*
        public static void Build(World world, Position startPosition, int sizeX, int sizeY, Rotation rotation, Random random)
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
                    Field field = world.GetField(x, y, World.BlocksLayerId);
                    if (field == null)
                    {
                        field = new Field();
                        world.SetField(x, y, World.BlocksLayerId, field);
                    }

                    new Floor(world, new Position(x, y), World.FloorLayerId);
                    if (x == startPosition.x || y == startPosition.y || x == startPosition.x + sizeX - 1 || y == startPosition.y + sizeY - 1)
                    {

                        if (rotation == Rotation.left || rotation == Rotation.right)
                        {
                            if (y != doorPosition)
                            {
                                new Wall(world, new Position(x, y));
                                if (random.Next(0, 15) == 0)
                                {
                                    new Ruine(world, new Position(x, y), World.BlocksLayerId);
                                }
                            }

                            else
                            {
                                new Door(world, new Position(x, y), World.BlocksLayerId);
                            }
                        }

                        else
                        {
                            if (x != doorPosition)
                            {
                                new Wall(world, new Position(x, y));
                                if (random.Next(0, 15) == 0)
                                {
                                    new Ruine(world, new Position(x, y), World.BlocksLayerId);
                                }
                            }
                            else
                            {
                                new Door(world, new Position(x, y), World.BlocksLayerId);
                            }
                        }
                    }
                    field.structureName = $"House #{startPosition.x + startPosition.y}";
                    field.isStructure = true;
                }
            }
        }
        */
    }
}
