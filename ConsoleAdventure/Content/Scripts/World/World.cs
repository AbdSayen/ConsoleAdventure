using System.Collections.Generic;
using System.Diagnostics;

namespace ConsoleAdventure.WorldEngine
{
    public class World
    {
        private int worldSize = 30;

        // [Z][Y][X]
        public List<List<List<Field>>> fields = new List<List<List<Field>>>();
        public List<Player> players = new List<Player>();
        public Stopwatch timer = new Stopwatch();

        private Generator generator;
        private Renderer renderer;

        public static int CountOfLayers = 4;
        public static int FloorLayerId = 0;
        public static int BlocksLayerId = 1;
        public static int MobsLayerId = 2;
        public static int ItemsLayerId = 3;
        public World()
        {
            generator = new Generator(this, worldSize);
            renderer = new Renderer(fields);
            generator.Generate(1);
            ConnectPlayer();
        }

        public void ConnectPlayer()
        {
            players.Add(new Player(players.Count, this, World.BlocksLayerId));
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
            switch (rotation)
            {
                case Rotation.up:
                    if (subject.position.y - stepSize >= 0 && fields[worldLayer][subject.position.x][subject.position.y - stepSize].content == null)
                    {
                        fields[worldLayer][subject.position.y][subject.position.x].content = null;
                        subject.position.SetPosition(subject.position.x, subject.position.y - stepSize);
                        fields[worldLayer][subject.position.y][subject.position.x].content = subject;
                    }
                    break;
                case Rotation.upRight:
                    if (subject.position.x + stepSize < fields[worldLayer].Count && subject.position.y - stepSize >= 0 && fields[worldLayer][subject.position.x + stepSize][subject.position.y - stepSize].content == null)
                    {
                        fields[worldLayer][subject.position.y][subject.position.x].content = null;
                        subject.position.SetPosition(subject.position.x + stepSize, subject.position.y - stepSize);
                        fields[worldLayer][subject.position.y][subject.position.x].content = subject;
                    }
                    break;
                case Rotation.right:
                    if (subject.position.x + stepSize < fields[worldLayer].Count && fields[worldLayer][subject.position.x + stepSize][subject.position.y].content == null)
                    {
                        fields[worldLayer][subject.position.y][subject.position.x].content = null;
                        subject.position.SetPosition(subject.position.x + stepSize, subject.position.y);
                        fields[worldLayer][subject.position.y][subject.position.x].content = subject;
                    }
                    break;
                case Rotation.rightDown:
                    if (subject.position.x + stepSize < fields[worldLayer].Count && subject.position.y + stepSize < fields[worldLayer][subject.position.x].Count && fields[worldLayer][subject.position.x + stepSize][subject.position.y + stepSize].content == null)
                    {
                        fields[worldLayer][subject.position.y][subject.position.x].content = null;
                        subject.position.SetPosition(subject.position.x + stepSize, subject.position.y + stepSize);
                        fields[worldLayer][subject.position.y][subject.position.x].content = subject;
                    }
                    break;
                case Rotation.down:
                    if (subject.position.y + stepSize < fields[worldLayer][subject.position.x].Count && fields[worldLayer][subject.position.x][subject.position.y + stepSize].content == null)
                    {
                        fields[worldLayer][subject.position.y][subject.position.x].content = null;
                        subject.position.SetPosition(subject.position.x, subject.position.y + stepSize);
                        fields[worldLayer][subject.position.y][subject.position.x].content = subject;
                    }
                    break;
                case Rotation.downLeft:
                    if (subject.position.x - stepSize >= 0 && subject.position.y + stepSize < fields[worldLayer][subject.position.x].Count && fields[worldLayer][subject.position.x - stepSize][subject.position.y + stepSize].content == null)
                    {
                        fields[worldLayer][subject.position.y][subject.position.x].content = null;
                        subject.position.SetPosition(subject.position.x - stepSize, subject.position.y + stepSize);
                        fields[worldLayer][subject.position.y][subject.position.x].content = subject;
                    }
                    break;
                case Rotation.left:
                    if (subject.position.x - stepSize >= 0 && fields[worldLayer][subject.position.x - stepSize][subject.position.y].content == null)
                    {
                        fields[worldLayer][subject.position.y][subject.position.x].content = null;
                        subject.position.SetPosition(subject.position.x - stepSize, subject.position.y);
                        fields[worldLayer][subject.position.y][subject.position.x].content = subject;
                    }
                    break;
                case Rotation.upLeft:
                    if (subject.position.x - stepSize >= 0 && subject.position.y - stepSize >= 0 && fields[worldLayer][subject.position.x - stepSize][subject.position.y - stepSize].content == null)
                    {
                        fields[worldLayer][subject.position.y][subject.position.x].content = null;
                        subject.position.SetPosition(subject.position.x - stepSize, subject.position.y - stepSize);
                        fields[worldLayer][subject.position.y][subject.position.x].content = subject;
                    }
                    break;
                default:
                    subject.position.SetPosition(subject.position.x, subject.position.y);
                    break;
            }
        }
    }
}
