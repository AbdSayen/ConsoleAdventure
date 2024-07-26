using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    public class PathFinder //дич, не юзайте
    {
        private static readonly Position[] Neighbors =
        {
            new Position(-1, 0), 
            new Position(1, 0),
            new Position(0, -1), 
            new Position(0, 1)
        };

        public static List<Position> FindPath(Position start, Position goal)
        {
            SortedSet<Node> openSet = new SortedSet<Node>(new NodeComparer());
            Dictionary<Position, Position> cameFrom = new Dictionary<Position, Position>();

            Dictionary<Position, int> gScore = new Dictionary<Position, int>();
            Dictionary<Position, int> fScore = new Dictionary<Position, int>();

            gScore[start] = 0;
            fScore[start] = Heuristic(start, goal);

            openSet.Add(new Node(start, fScore[start]));

            while (openSet.Count > 0)
            {
                Position current = openSet.Min.Position;

                if (current.Equals(goal))
                    return ReconstructPath(cameFrom, current);

                openSet.Remove(openSet.Min);

                foreach (Position neighborOffset in Neighbors)
                {
                    Position neighbor = new Position(current.x + neighborOffset.x, current.y + neighborOffset.y);

                    Field fBlock = ConsoleAdventure.world.GetField(neighbor.x, neighbor.y, World.BlocksLayerId);
                    Field fEntity = ConsoleAdventure.world.GetField(neighbor.x, neighbor.y, World.MobsLayerId);

                    Transform block = fBlock != null ? fBlock.content : null;
                    Transform entity = fEntity != null ? fEntity.content : null;

                    bool isBlock = block != null ? block.isObstacle : false;
                    bool isEntity = entity != null ? entity.type > 0 : false;

                    if (!IsValidPosition(neighbor) || isBlock || isEntity)
                        continue;

                    int tentativeGScore = gScore[current] + 1;

                    if (!gScore.ContainsKey(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = gScore[neighbor] + Heuristic(neighbor, goal);

                        if (!openSet.Contains(new Node(neighbor, fScore[neighbor])))
                            openSet.Add(new Node(neighbor, fScore[neighbor]));
                    }
                }
            }

            return null; //Пути нету(
        }

        private static int Heuristic(Position a, Position b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        private static bool IsValidPosition(Position pos)
        {
            return pos.x >= 0 && pos.x < ConsoleAdventure.world.size && pos.y >= 0 && pos.y < ConsoleAdventure.world.size;
        }

        private static List<Position> ReconstructPath(Dictionary<Position, Position> cameFrom, Position current)
        {
            var path = new List<Position>();
            while (cameFrom.ContainsKey(current))
            {
                path.Add(current);
                current = cameFrom[current];
            }
            path.Add(current);
            path.Reverse();
            return path;
        }

        private class Node
        {
            public Position Position { get; }
            public int FScore { get; }

            public Node(Position position, int fScore)
            {
                Position = position;
                FScore = fScore;
            }
        }

        private class NodeComparer : IComparer<Node>
        {
            public int Compare(Node x, Node y)
            {
                int compare = x.FScore.CompareTo(y.FScore);
                if (compare == 0)
                    compare = x.Position.GetHashCode().CompareTo(y.Position.GetHashCode());
                return compare;
            }
        }
    }
}
