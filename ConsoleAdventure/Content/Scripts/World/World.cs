using ConsoleAdventure.WorldEngine.Generate;
using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleAdventure.WorldEngine
{
    public class World
    {
        private int worldSize = 300;

        // [Z][Y][X]
        public List<List<List<Field>>> fields = new List<List<List<Field>>>();
        public List<Player> players = new List<Player>();
        private Stopwatch timer = new Stopwatch();

        public Time time = new Time();
        private Generator generator;
        private Renderer renderer;
        private int seed = 1234;

        public static int CountOfLayers = 4;
        public static int FloorLayerId = 0;
        public static int BlocksLayerId = 1;
        public static int MobsLayerId = 2;
        public static int ItemsLayerId = 3;

        public World()
        {
            generator = new Generator(this, worldSize);
            renderer = new Renderer(fields);
            ConnectPlayer();
            generator.Generate(seed);
        }

        public void ConnectPlayer()
        {
            players.Add(new Player(players.Count, this, new Position(0, 0), World.MobsLayerId));
        }

        public void ListenEvents()
        {
            timer.Start();
            for (int i = 0; i < players.Count && timer.Elapsed.TotalMilliseconds > 25; i++)
            {
                players[i].CheckMove();
                timer.Restart();
            }
        }

        public string Render()
        {
            return $"{renderer.Render(players[0])}";
            //return $"{renderer.PrimitiveRender()}";
        }

        public void MoveSubject(Transform subject, int worldLayer, int stepSize, Rotation rotation)
        {
            int newX = subject.position.x;
            int newY = subject.position.y;

            switch (rotation)
            {
                case Rotation.up:
                    newY -= stepSize;
                    break;
                case Rotation.right:
                    newX += stepSize;
                    break;
                case Rotation.down:
                    newY += stepSize;
                    break;
                case Rotation.left:
                    newX -= stepSize;
                    break;
                default:
                    break;
            }

            if (IsValidMove(worldLayer, newX, newY))
            {
                fields[worldLayer][subject.position.y][subject.position.x].content = null;
                subject.position.SetPosition(newX, newY);
                fields[worldLayer][newY][newX].content = subject;
                time.PassTime(3);
            }

            bool IsValidMove(int worldLayer, int newX, int newY)
            {
                return newX >= 0 && newX < fields[worldLayer].Count &&
                       newY >= 0 && newY < fields[worldLayer][newX].Count &&
                       (fields[worldLayer][newY][newX].content == null && (fields[BlocksLayerId][newY][newX].content == null ||
                       !fields[BlocksLayerId][newY][newX].content.isObstacle));
            }
        }
    }
}
