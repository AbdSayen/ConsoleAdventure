using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.Entitys;
using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine.Generate;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;


namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class World
    {
        public int size { get; internal set; } = 256;

        public List<List<Chunk>> chunks = new List<List<Chunk>>();
        public List<Player> players = new List<Player>();
        public List<Entity> entitys = new List<Entity>();

        public Time time = new Time();

        [NonSerialized]
        private Generator generator;

        [NonSerialized]
        private Renderer renderer;

        public int seed = 1234;

        public static int CountOfLayers = 4;
        public static int FloorLayerId = 0;
        public static int BlocksLayerId = 1;
        public static int ItemsLayerId = 2;
        public static int MobsLayerId = 3;

        public World()
        {
            ConsoleAdventure.rand = new Random();

            generator = new Generator(this, size);
            renderer = new Renderer(chunks);
            generator.Generate(seed);
            ConnectPlayer();

            entitys.Add(new Cat(this, new(6, 6)));
            entitys.Add(new Cat(this, new(6, 7)));
            entitys.Add(new Cat(this, new(6, 8)));
            entitys.Add(new Cat(this, new(6, 9)));
            entitys.Add(new Cat(this, new(7, 6)));
            entitys.Add(new Cat(this, new(7, 7)));
            entitys.Add(new Cat(this, new(7, 8)));
            entitys.Add(new Cat(this, new(7, 9)));
            entitys.Add(new Cat(this, new(8, 6)));
            entitys.Add(new Cat(this, new(8, 7)));
            entitys.Add(new Cat(this, new(8, 8)));
            entitys.Add(new Cat(this, new(8, 9)));
            entitys.Add(new Cat(this, new(9, 6)));
            entitys.Add(new Cat(this, new(9, 7)));
            entitys.Add(new Cat(this, new(9, 8)));
            entitys.Add(new Cat(this, new(9, 9)));
        }

        public Point GetCunkCounts()
        {
            return new(chunks[0].Count, chunks.Count);
        }

        public void ConnectPlayer()
        {
            players.Add(new Player(players.Count, this, new Position(5, 5)));
        }
        
        public void ListenEvents()
        {
            if (!ConsoleAdventure.isPause)
            {
                time.PassTime(1);

                for (int i = 0; i < players.Count; i++)
                {
                    players[i].InteractWithWorld();
                }

                for (int i = 0; i < entitys.Count; i++)
                {
                    entitys[i].InteractWithWorld();
                }
            }         
        }

        public void Render()
        {
            renderer.Render(players[0], players[0].cursorPosition);
        }

        public void RemoveSubject(Transform subject, int worldLayer, bool isDroped = true)
        {
            if (isDroped && subject != null && worldLayer >= 0 && worldLayer <= CountOfLayers)
                GetField(subject.position.x, subject.position.y, worldLayer).content.Collapse();       
            if(subject != null && worldLayer >= 0 && worldLayer <= CountOfLayers)
                GetField(subject.position.x, subject.position.y, worldLayer).content = null;  
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

            SetSubjectPosition(subject, worldLayer, newX, newY);
        }

        public void SetSubjectPosition(Transform subject, int worldLayer, int newX, int newY)
        {
            if (IsValidMove(worldLayer, newX, newY))
            {
                GetField(subject.position.x, subject.position.y, worldLayer).content = null;
                subject.position.SetPosition(newX, newY);
                GetField(newX, newY, worldLayer).content = subject;
                GetField(newX, newY, worldLayer).color = subject.GetColor();
                //time.PassTime(3);
            }

            bool IsValidMove(int worldLayer, int newX, int newY)
            {
                return newX >= 0 && newX < size &&
                       newY >= 0 && newY < size &&
                       (GetField(newX, newY, worldLayer).content == null &&
                       (GetField(newX, newY, BlocksLayerId).content == null ||
                       !GetField(newX, newY, BlocksLayerId).content.isObstacle));
            }
        }

        public Field GetField(int x, int y, int layer)
        {
            int chunkX = x / Chunk.Size;
            int chunkY = y / Chunk.Size;
            int localX = x % Chunk.Size;
            int localY = y % Chunk.Size;

            if (chunkX >= 0 && chunkX < chunks.Count && chunkY >= 0 && chunkY < chunks[chunkX].Count)
            {
                return chunks[chunkX][chunkY].GetField(localX, localY, layer);
            }

            return null;
        }

        public List<List<Field>> GetFields(int y, int layer)
        {
            int chunkY = y / Chunk.Size;
            int localY = y % Chunk.Size;

            if (chunkY >= 0 && chunkY < chunks.Count)
            {
                var result = new List<List<Field>>();
                foreach (var chunkRow in chunks)
                {
                    foreach (var chunk in chunkRow)
                    {
                        result.Add(chunk.GetFields(layer)[localY]);
                    }
                }
                return result;
            }

            return null;
        }

        public List<List<Field>> GetFields(int layer)
        {
            var result = new List<List<Field>>();
            foreach (var chunkRow in chunks)
            {
                foreach (var chunk in chunkRow)
                {
                    var fields = chunk.GetFields(layer);
                    foreach (var row in fields)
                    {
                        result.Add(row);
                    }
                }
            }
            return result;
        }

        public List<List<List<Field>>> GetFields()
        {
            var result = new List<List<List<Field>>>();
            foreach (var chunkRow in chunks)
            {
                foreach (var chunk in chunkRow)
                {
                    result.AddRange(chunk.GetFields());
                }
            }
            return result;
        }

        public void SetField(int x, int y, int layer, Field field)
        {
            int chunkX = x / Chunk.Size;
            int chunkY = y / Chunk.Size;
            int localX = x % Chunk.Size;
            int localY = y % Chunk.Size;

            if (chunkX >= 0 && chunkX < chunks.Count && chunkY >= 0 && chunkY < chunks[chunkX].Count)
            {
                chunks[chunkX][chunkY].SetField(localX, localY, layer, field);
            }
        }

        public List<List<Chunk>> GetChunks()
        {
            return chunks;
        }

        public void InitializeChunks()
        {
            int chunkCount = (size + Chunk.Size - 1) / Chunk.Size;
            for (int y = 0; y < chunkCount; y++)
            {
                chunks.Add(new List<Chunk>());
                for (int x = 0; x < chunkCount; x++)
                {
                    chunks[y].Add(new Chunk());
                }
            }
        }
    }
}
