using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine.Generate;
using System;
using System.Collections.Generic;
using ConsoleAdventure.Content.Scripts.Player;
using ConsoleAdventure.Content.Scripts.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using ConsoleAdventure.Content.Scripts.Debug.Commands;
using ConsoleAdventure.Content.Scripts.InputLogic;
using System.Text;


namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class World
    {
        public Action Start;
        
        public int size { get; internal set; } = 256;

        public List<List<Chunk>> chunks = new List<List<Chunk>>();
        public List<Player> players = new List<Player>();
        public List<Entity> entities = new List<Entity>();

        public Time time = new Time();

        [NonSerialized]
        public Generator generator;

        [NonSerialized]
        private Renderer renderer;

        public string name;
        public int seed = 1234;

        public readonly static int CountOfLayers = 4;
        public readonly static int FloorLayerId = 0;
        public readonly static int BlocksLayerId = 1;
        public readonly static int ItemsLayerId = 2;
        public readonly static int MobsLayerId = 3;

        internal bool isInitialized = false;

        private bool _isFirstFrame = true;

        public bool isCmdOpen;

        TextInputField inputField;

        public World(string name, int seed)
        {
            this.name = name;
            this.seed = seed;

            ConsoleAdventure.rand = new Random();

            generator = new Generator(this, size);
            renderer = new Renderer(chunks);
            new Cursor();

            inputField = new TextInputField(new Point(0, 0), Color.White, 173, 0, "Введите комманду...", 0, new char[1] { '\r' });
            inputField.Position = new Vector2(18, ConsoleAdventure.Height - (19 * 2) - 19);
            inputField.isHover = true;
        }

        public void Initialize(bool isfullGenerate = true)
        {
            if (!isInitialized)
            {
                generator.Generate(seed, isfullGenerate);
                ConnectPlayer();

                CaModLoader.WorldLoadedMods(this);
                isInitialized = true;
            }
        }

        public Point GetChunkCounts()
        {
            return new(chunks[0].Count, chunks.Count);
        }

        public void ConnectPlayer()
        {
            players.Add(new Player(players.Count, new Position(5, 5), ConsoleAdventure.StartDeep));
        }

        int timer;
        public void ListenEvents()
        {
            if (!ConsoleAdventure.isPause)
            {
                if (_isFirstFrame)
                {
                    Start?.Invoke();
                    _isFirstFrame = false;
                }
                
                time.PassTime(1);

                for (int i = 0; i < players.Count; i++)
                {
                    players[i].InteractWithWorld();
                }

                for (int i = 0; i < entities.Count; i++)
                {
                    entities[i].InteractWithWorld();
                }
            }

            if (timer > (1 * 60 * 60))
            {
                WorldIO.Save("World");

                timer = 0;
            }

            if (!ConsoleAdventure.kstate.IsKeyDown(InputConfig.Cmd) && ConsoleAdventure.prekstate.IsKeyDown(InputConfig.Cmd))
            {
                if (isCmdOpen)
                {
                    isCmdOpen = false;
                    ConsoleAdventure.BlockHotKey = false;
                    inputField.isHover = false;
                }

                else
                {
                    isCmdOpen = true;
                    ConsoleAdventure.BlockHotKey = true;
                    inputField.isHover = true;
                }
            }

            timer++;
        }

        public void Render()
        {
            renderer.Render(players[0], Cursor.Instance.CursorPosition);

            if (isCmdOpen)
            {
                inputField.Update();
                inputField.Draw(ConsoleAdventure._spriteBatch);

                if (ConsoleAdventure.kstate.IsKeyDown(Keys.Enter))
                {
                    Command.Find(inputField.text);
                    inputField.text = "";
                    inputField.cursorPos = new();
                }
            }
        }

        public void RemoveSubject(Transform subject, int worldLayer, bool isDroped = true)
        {
            Field field = subject != null ? GetField(subject.position.x, subject.position.y, worldLayer, subject.w) : null;

            if (isDroped && field != null && field.content != null && worldLayer >= 0 && worldLayer <= CountOfLayers)
                field.content.Collapse();       
            if(field != null && worldLayer >= 0 && worldLayer <= CountOfLayers)
                field.content = null;  
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

        public void MoveSubject(Transform subject, int worldLayer, int stepSize, Position position)
        {
            int newX = subject.position.x;
            int newY = subject.position.y;

            if (position.y > 0)
            {
                newY -= stepSize;
            }
            if (position.y < 0)
            {
                newY += stepSize;
            }
            if (position.x > 0)
            {
                newX += stepSize;
            }
            if (position.x < 0)
            {
                newX -= stepSize;
            }

            SetSubjectPosition(subject, worldLayer, newX, newY);
        }

        public bool SetSubjectPosition(Transform subject, int worldLayer, int newX, int newY, int newW = -1)
        {
            int deep = (newW == -1) ? subject.w : newW;

            if (IsValidMove(worldLayer, newX, newY, deep))
            {
                GetField(subject.position.x, subject.position.y, worldLayer, subject.w).content = null;
                subject.position.SetPosition(newX, newY);
                subject.w = deep;
                GetField(newX, newY, worldLayer, deep).content = subject;
                GetField(newX, newY, worldLayer, deep).color = subject.GetColor();
                return true;
            }

            bool IsValidMove(int worldLayer, int newX, int newY, int w)
            {
                return newX >= 0 && newX < size &&
                       newY >= 0 && newY < size &&
                       (GetField(newX, newY, worldLayer, w).content == null &&
                       (GetField(newX, newY, BlocksLayerId, w).content == null ||
                       !GetField(newX, newY, BlocksLayerId, w).content.isObstacle));
            }

            return false;
        }

        public Field GetField(int x, int y, int layer, int w)
        {
            int chunkX = x / Chunk.Size;
            int chunkY = y / Chunk.Size;
            int localX = x % Chunk.Size;
            int localY = y % Chunk.Size;

            if (chunkX >= 0 && chunkX < chunks.Count && chunkY >= 0 && chunkY < chunks[chunkX].Count)
            {
                Field field = chunks[chunkX][chunkY].GetField(localX, localY, layer, w);
                return field;
            }

            return new();
        }

        public List<List<Field>> GetFields(int y, int layer, int w)
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
                        result.Add(chunk.GetFields(layer, w)[localY]);
                    }
                }
                return result;
            }

            return null;
        }

        public List<List<Field>> GetFields(int layer, int w)
        {
            var result = new List<List<Field>>();
            foreach (var chunkRow in chunks)
            {
                foreach (var chunk in chunkRow)
                {
                    var fields = chunk.GetFields(layer, w);
                    foreach (var row in fields)
                    {
                        result.Add(row);
                    }
                }
            }
            return result;
        }

        public List<List<List<Field>>> GetFields(int w)
        {
            var result = new List<List<List<Field>>>();
            foreach (var chunkRow in chunks)
            {
                foreach (var chunk in chunkRow)
                {
                    result.AddRange(chunk.GetFields(w));
                }
            }
            return result;
        }

        public List<List<List<List<Field>>>> GetFields()
        {
            var result = new List<List<List<List<Field>>>>();
            foreach (var chunkRow in chunks)
            {
                foreach (var chunk in chunkRow)
                {
                    result.AddRange(chunk.GetFields());
                }
            }
            return result;
        }

        public void SetField(int x, int y, int layer, int w, Field field)
        {
            int chunkX = x / Chunk.Size;
            int chunkY = y / Chunk.Size;
            int localX = x % Chunk.Size;
            int localY = y % Chunk.Size;

            if (chunkX >= 0 && chunkX < chunks.Count && chunkY >= 0 && chunkY < chunks[chunkX].Count)
            {
                chunks[chunkX][chunkY].SetField(localX, localY, layer, w, field);
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

        public string LevelToString(int w)
        {
            StringBuilder result = new StringBuilder();

            for (int i = 0; i < size; i++) //y
            {
                for (int j = 0; j < size; j++) //x
                {
                    Field field = GetField(j, i, 1, w);

                    if (field?.content != null)
                    {
                        result.Append(field.content.GetSymbol());
                    }
                    else
                    {
                        result.Append("  ");
                    }
                }

                result.Append("\r\n");
            }

            return result.ToString();
        }
    }
}
