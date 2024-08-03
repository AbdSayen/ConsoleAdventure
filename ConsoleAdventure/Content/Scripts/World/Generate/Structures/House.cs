using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ConsoleAdventure.Generate.Structures
{
    public class House : Structure
    {
        private Random random = ConsoleAdventure.rand;
        private World world = ConsoleAdventure.world;
        private List<Room> rooms = new List<Room>();

        public House(Position startPosition, int sizeX, int sizeY)
        {
            Room initialRoom = new Room(
                startPosition,
                new Position(startPosition.x + sizeX, startPosition.y),
                new Position(startPosition.x, startPosition.y + sizeY),
                new Position(startPosition.x + sizeX, startPosition.y + sizeY)
            );
            rooms.Add(initialRoom);

            CreateDivision(rooms[0], random.Next(1, 3));
            CreateDivision(rooms[1], random.Next(1, 3));

            for (int y = startPosition.y; y < startPosition.y + sizeY; y++)
            {
                for (int x = startPosition.x; x < startPosition.x + sizeX; x++)
                {
                    new Floor(new Position(x, y), World.FloorLayerId);
                    world.GetField(x, y, World.BlocksLayerId).isStructure = true;

                    bool isHorizontalWall = (y == startPosition.y || y == startPosition.y + sizeY - 1);
                    bool isVerticalWall = (x == startPosition.x || x == startPosition.x + sizeX - 1);
                    bool isWall = isHorizontalWall || isVerticalWall;

                    if (isWall)
                    {
                        new Wall(new Position(x, y));
                        if (isHorizontalWall && x == (startPosition.x + sizeX / 2) && sizeX > 3)
                        {
                            new Door(new Position(x, y));
                            new Door(new Position(x + 1, y));
                        }
                        else if (isVerticalWall && y == (startPosition.y + sizeY / 2) && sizeY > 3)
                        {
                            new Door(new Position(x, y));
                            new Door(new Position(x, y + 1));
                        }
                    }

                    if (world.GetField(x, y, World.BlocksLayerId).content != null && random.Next(0, 8) == 0)
                    {
                        new Ruine(new Position(x, y));
                    }
                    else if (random.Next(0, 320) == 0)
                    {
                        new Cat(new Position(x, y));
                    }
                }
            }
        }

        private void CreateDivision(Room parentRoom, int count = 1)
        {
            int width = parentRoom.Corners[1].x - parentRoom.Corners[0].x;
            int height = parentRoom.Corners[2].y - parentRoom.Corners[0].y;
            int minSlice = (int)(0.25 * (width > height ? width : height));
            int maxSlice = (int)(0.45 * (width > height ? width : height));
            int minDistance = 5;

            Position startPosition = parentRoom.Corners[0];
            Position endPosition = parentRoom.Corners[3];

            for (int i = 0; i < count; i++)
            {
                bool sliceCreated = false;

                if (width > height)
                {
                    int sliceX;
                    bool validSlice = false;
                    int attempts = 0;
                    do
                    {
                        sliceX = random.Next(startPosition.x + minSlice, endPosition.x - minSlice);
                        validSlice = true;


                        attempts++;
                        if (attempts > 50) break;
                        foreach (Room room in rooms)
                        {
                            if (Math.Abs(sliceX - room.Corners[0].x) < minDistance || Math.Abs(sliceX - room.Corners[1].x) < minDistance)
                            {
                                validSlice = false;
                                break;
                            }
                        }
                    } while (!validSlice);

                    if (sliceX >= startPosition.x + minSlice && sliceX <= endPosition.x - minSlice)
                    {
                        for (int y = startPosition.y; y < endPosition.y; y++)
                        {
                            new Plank(new Position(sliceX, y));
                        }

                        int doorY = startPosition.y + (endPosition.y - startPosition.y) / 2;
                        new Door(new Position(sliceX, doorY));

                        sliceCreated = true;

                        Room leftRoom = new Room(startPosition, new Position(sliceX, startPosition.y), new Position(startPosition.x, endPosition.y), new Position(sliceX, endPosition.y));
                        Room rightRoom = new Room(new Position(sliceX + 1, startPosition.y), endPosition, new Position(sliceX + 1, startPosition.y), endPosition);

                        if (leftRoom.GetArea() > rightRoom.GetArea())
                        {
                            parentRoom.Corners[1] = leftRoom.Corners[1];
                            parentRoom.Corners[3] = leftRoom.Corners[3];
                            rooms.Add(rightRoom);
                        }
                        else
                        {
                            parentRoom.Corners[0] = rightRoom.Corners[0];
                            parentRoom.Corners[2] = rightRoom.Corners[2];
                            rooms.Add(leftRoom);
                        }

                        width = parentRoom.Corners[1].x - parentRoom.Corners[0].x;
                    }
                }
                else
                {
                    int sliceY;
                    bool validSlice = false;
                    int attempts = 0;

                    do
                    {
                        sliceY = random.Next(startPosition.y + minSlice, endPosition.y - minSlice);
                        validSlice = true;

                        attempts++;
                        if (attempts > 50) break;

                        foreach (Room room in rooms)
                        {
                            if (Math.Abs(sliceY - room.Corners[0].y) < minDistance || Math.Abs(sliceY - room.Corners[2].y) < minDistance)
                            {
                                validSlice = false;
                                break;
                            }
                        }
                    } while (!validSlice);

                    if (sliceY >= startPosition.y + minSlice && sliceY <= endPosition.y - minSlice)
                    {
                        for (int x = startPosition.x; x < endPosition.x; x++)
                        {
                            new Plank(new Position(x, sliceY));
                        }

                        int doorX = startPosition.x + (endPosition.x - startPosition.x) / 2;
                        new Door(new Position(doorX, sliceY));

                        sliceCreated = true;

                        Room topRoom = new Room(startPosition, new Position(endPosition.x, startPosition.y), new Position(startPosition.x, sliceY), new Position(endPosition.x, sliceY));
                        Room bottomRoom = new Room(new Position(startPosition.x, sliceY + 1), endPosition, new Position(startPosition.x, sliceY + 1), endPosition);

                        if (topRoom.GetArea() > bottomRoom.GetArea())
                        {
                            parentRoom.Corners[2] = topRoom.Corners[2];
                            parentRoom.Corners[3] = topRoom.Corners[3];
                            rooms.Add(bottomRoom);
                        }
                        else
                        {
                            parentRoom.Corners[0] = bottomRoom.Corners[0];
                            parentRoom.Corners[1] = bottomRoom.Corners[1];
                            rooms.Add(topRoom);
                        }

                        height = parentRoom.Corners[2].y - parentRoom.Corners[0].y;
                    }
                }

                if (!sliceCreated)
                {
                    break;
                }
            }
        }

        private Position FindCorner(Position position)
        {
            Position? nearestCorner = null;
            int minDistance = int.MaxValue;

            foreach (var room in rooms)
            {
                foreach (var corner in room.Corners)
                {
                    if (corner.x == position.x || corner.y == position.y)
                    {
                        int distance = Math.Abs(corner.x - position.x) + Math.Abs(corner.y - position.y);
                        if (distance < minDistance)
                        {
                            minDistance = distance;
                            nearestCorner = corner;
                        }
                    }
                }
            }

            if (nearestCorner.HasValue)
            {
                return nearestCorner.Value;
            }
            else
            {
                return position;
            }
        }

        private bool IsOnRoomBorder(Room room, int x, int y)
        {
            if ((y == room.Corners[0].y && x >= room.Corners[0].x && x <= room.Corners[1].x) ||
                (y == room.Corners[2].y && x >= room.Corners[2].x && x <= room.Corners[3].x))
            {
                return true;
            }

            if ((x == room.Corners[0].x && y >= room.Corners[0].y && y <= room.Corners[2].y) ||
                (x == room.Corners[1].x && y >= room.Corners[1].y && y <= room.Corners[3].y))
            {
                return true;
            }

            return false;
        }

        internal class Room
        {
            public Position[] Corners { get; }

            public Room(Position topLeft, Position topRight, Position bottomLeft, Position bottomRight)
            {
                Corners = new Position[4] { topLeft, topRight, bottomLeft, bottomRight };
            }

            public static Position[] GetAllCorners(List<Room> rooms)
            {
                var uniqueCorners = new HashSet<Position>(rooms.SelectMany(room => room.Corners));
                return uniqueCorners.ToArray();
            }

            public int GetArea()
            {
                int width = Corners[1].x - Corners[0].x;
                int height = Corners[2].y - Corners[0].y;
                return width * height;
            }
        }
    }
}