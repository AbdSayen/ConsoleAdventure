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
using ConsoleAdventure.Content.Scripts.WorldEngine.Events;
using System.Threading.Tasks;
using ConsoleAdventure.Networks;
using ConsoleAdventure.Content.Scripts.WorldEngine;
using SharpDX.Direct2D1;
using ConsoleAdventure.WorldEngine.Levels;
using CaModLoaderAPI;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using ConsoleAdventure.Content.Scripts.WorldEngine.ChunkManagement;

namespace ConsoleAdventure.WorldEngine
{
    public class World
    {
        public Action Start;
        
        public int size { get; internal set; } = 256 * 10;

        public Chunk[,] chunks;
        public Dictionary<short, Player> players = new();
        public List<Entity> entities = new List<Entity>();

        public Time time = new Time();
        public int timeSpeed = 1;

        public Generator generator;
        private Renderer renderer;
        
        public string name;
        public int seed;

        public static byte CountOfLayers { get; private set; } = 4;
        public static byte FloorLayerId { get; private set; } = 0;
        public static byte BlocksLayerId { get; private set; } = 1;
        public static byte ItemsLayerId { get; private set; } = 2;
        public static byte MobsLayerId { get; private set; } = 3;

        internal bool isInitialized = false;

        public bool isLoaded = false;

        private bool _isFirstFrame = true;

        public bool isCmdOpen;

        TextInputField inputField;

        public bool inMultiplayer = false;

        public Dictionary<string, byte[]> playersDat;

        internal Dictionary<string, int> modTransforms = new Dictionary<string, int>();

        public Rain rain = new Rain();

        public Position observerPos = new Position();
        public int observerW = 0;

        public int spawnW = 1;

        public Position? lastLoadedChunk = null;

        public WorldLevelsSystem levels;

        public Tags generationProperties = new Tags();

        public int Surface { get; private set; }

        public int Sedimentary { get; private set; }

        public int Cavern { get; private set; }

        public int LavaCavern { get; private set; }

        public World(string name, int seed, int size, bool isInitedContent = true, bool isGenerate = true)
        {
            this.name = name;
            this.seed = seed;
            this.size = (size / Chunk.Size) * Chunk.Size;

            levels = new WorldLevelsSystem(null);
            SetDeeps();

            generator = new Generator(this, size, isInitedContent, isGenerate);
            renderer = new Renderer();

            //ChunkManager.Start(this);
            ChunkManager.world = this;

            new Cursor();

            playersDat = new Dictionary<string, byte[]>();

            inputField = new TextInputField(new Point(0, 0), Color.White, 173, 0, "Введите текст...", 0, new char[1] { '\r' });
            inputField.Position = new Vector2(18, ConsoleAdventure.Height - (19 * 2) - 19);
            inputField.isHover = true;
        }

        public void SetDeeps()
        {
            Surface = levels.GetLevel(new Surface());
            Sedimentary = levels.GetLevel(new Sedimentary());
            Cavern = levels.GetLevel(new Cavern());
            LavaCavern = levels.GetLevel(new LavaCavern());

            foreach (Mod mod in CaModLoader.GetActiveMods())
            {
                mod.SetModDeeps();
            }
        }

        public async Task Initialize(bool isFullGenerate = true)
        {
            if (!isInitialized)
            {
                ConsoleAdventure.menu.State = MenuState.worldLoadingProgress;
                ConsoleAdventure.menu.worldErrorList = null;
                await Task.Run(() => generator.CreateWorld(seed, isFullGenerate));
                //ConsoleAdventure.menu.State = MenuState.worldMenu;
                CaModLoader.WorldPostGenerateMods(this);
                LoadInMultiplayer();

                if (players.Count > 0)
                {
                    GameEvent.InitEvents();
                    isInitialized = true;
                }
            }
        }

        public void Loaded()
        {
            isLoaded = true;

            if (playersDat.ContainsKey(NetworkManager.pcId))
            {
                GetLocalPlayer().LoadPlayerFromBytes(playersDat[NetworkManager.pcId]);
            }

            CaModLoader.WorldLoadedMods(this);
        }

        private async void LoadInMultiplayer()
        {
            if (!inMultiplayer)
            {
                ConnectPlayer(0, NetworkManager.pcId);
                players[0].isActive = true;
                return;
            }
            if (await NetworkManager.ConnectClient())
            {
                ConnectLocalPlayer();
                await NetworkManager.ImConnected();
                await NetworkManager.RequestPlayersData();
            }
        }

        public Point GetChunkCounts()
        {
            return new(ConsoleAdventure.world.chunks.GetLength(0), ConsoleAdventure.world.chunks.GetLength(1));
        }

        public void ConnectLocalPlayer()
        {
            ConsoleAdventure.logger.AddMessage("Creating local player");
            short id = NetworkManager.Id;
            ConnectPlayer(id, NetworkManager.pcId);
            GetLocalPlayer().isActive = true;

            for (int i = 0; i < CaModLoader.modGlobalPlayers.Count; i++)
            {
                CaModLoader.modGlobalPlayers[i].PostConnect(GetLocalPlayer());
            }
        }

        public void ConnectPlayer(short id, string pcId = "")
        {
            Player player = new Player(id, pcId, new Position(5 + id, 5 + id), ConsoleAdventure.world.Surface);
            players.Add(player.info.Id, player);
        }

        public void DisconnectPlayer(short id)
        {
            players[id].Kill();
            players.Remove(id);
        }

        public Player GetLocalPlayer()
        {
            if (NetworkManager.Id != -1)
                return players[NetworkManager.Id];
            return players[0];
        }

        int timer;
        public void ListenEvents()
        {
            if (!ConsoleAdventure.isPause)
            {
                //GetLocalPlayer().w = 3;
                CaModLoader.PreWorldUpdateMods(this);

                if (_isFirstFrame)
                {
                    Start?.Invoke();
                    _isFirstFrame = false;
                }
                
                time.PassTime(timeSpeed);

                GetLocalPlayer().UpdateEntityInWorld();

                for (int i = 0; i < entities.Count; i++)
                {
                    entities[i].UpdateEntityInWorld();
                }

                //for (int i = 0; i < 20; i++)
                //{
                //    Field field = GetField(ConsoleAdventure.rand.Next(0, size), ConsoleAdventure.rand.Next(0, size), ConsoleAdventure.rand.Next(0, CountOfLayers), ConsoleAdventure.rand.Next(0, Chunk.maxDeep));
                //    if (field?.content != null)
                //    {
                //        field.content.RandomUpdate();
                //    }
                //}

                for (int i = 0; i < GameEvent.Events.Count; i++)
                {
                    if (GameEvent.Events[i].IsActive())
                    {
                        GameEvent.Events[i].Update();
                    }
                }

                Spawner.Update();

                CaModLoader.PostWorldUpdateMods(this);
            }

            if (timer > (10 * 60 * 60) && NetworkManager.Id <= 0)
            {
                WorldIO.Save(name);

                timer = 0;
            }

            if (Input.PostClick(InputConfig.Cmd))
            {
                if (!isCmdOpen)
                {
                    isCmdOpen = true;
                    ConsoleAdventure.BlockHotKey = true;
                    inputField.isHover = true;
                }
            }

            if (Input.PostClick(InputConfig.WorldExit))
            {
                //ChunkManager.End();

                if (isCmdOpen)
                {
                    isCmdOpen = false;
                    ConsoleAdventure.BlockHotKey = false;
                    inputField.isHover = false;
                }
            }

            Loot.blinkTimer++;
            timer++;
        }

        public void Render()
        {
            Observer observer = CaModLoader.GetWorldObserverMods(this);
            if (observer == null)
            {
                Player p = GetLocalPlayer();
                observer = new Observer(p.position, p.w);
            }
            renderer.Render(observer.position, observer.w, Cursor.Instance.CursorPosition, Color.OrangeRed);

            ChatDraw();
        }

        public void ChatDraw()
        {
            if (isCmdOpen)
            {
                inputField.Update();
                inputField.Draw(ConsoleAdventure._spriteBatch);

                if (ConsoleAdventure.kstate.IsKeyDown(Keys.Enter) && inputField.text != "")
                {
                    if (inputField.text[0] == '/')
                    {
                        Command.Find(inputField.text.Remove(0, 1));
                    }
                    else
                    {
                        string pre = "You";
                        Loger.AddLog(Utils.StringMaxLengthOnLine(pre + ": " + inputField.text, 24));
                        NetworkManager.SendChatMessage(inputField.text);
                    }
                    inputField.text = "";
                    inputField.cursorPos = new();
                    isCmdOpen = false;
                    ConsoleAdventure.BlockHotKey = false;
                }
            }
        }

        public void RemoveSubject(Transform subject, int worldLayer, bool isDroped = true)
        {
            Field field = subject != null ? GetField(subject.position.x, subject.position.y, worldLayer, subject.w) : null;

            if (isDroped && field != null && field.content != null && worldLayer >= 0 && worldLayer <= CountOfLayers)
            {
                if (CaModLoader.PreCollapseMods(field?.content))
                    field.content.Collapse();
            }

            if (field != null && worldLayer >= 0 && worldLayer <= CountOfLayers)
                field.Destroy();
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

        public bool SetSubjectPosition(Transform subject, int worldLayer, int newX, int newY, byte? newW = null)
        {
            byte deep = (newW == null) ? subject.w : (byte)newW;

            Field field0 = GetField(subject.position.x, subject.position.y, worldLayer, subject.w);
            Field field1 = GetField(newX, newY, worldLayer, deep);
            Chunk chunk = GetChunk(newX, newY, out int v1, out int v2);

            if (IsValidMove(worldLayer, newX, newY, deep) && ((field0 != null && field1 != null) || chunk == null || chunk is UnloadedChunk))
            {
                field0.content = null;
                subject.position.SetPosition(newX, newY);
                subject.w = deep;
                field1.content = subject;
                return true;
            }

            bool IsValidMove(int worldLayer, int newX, int newY, int w)
            {
                Transform block = GetField(newX, newY, BlocksLayerId, w)?.content;

                return newX >= 0 && newX < size &&
                       newY >= 0 && newY < size &&
                       (GetField(newX, newY, worldLayer, w)?.content == null &&
                       (block == null ||
                       (!Transform.IsObstacle[block] || ConsoleAdventure.NoCollision)));
            }

            return false;
        }

        public Field GetField(int x, int y, int layer, int w)
        {
            int chunkX = x / Chunk.Size;
            int chunkY = y / Chunk.Size;
            int localX = x % Chunk.Size;
            int localY = y % Chunk.Size;

            if (ConsoleAdventure.world?.chunks == null) return new();
            if (chunkX >= 0 && chunkX < ConsoleAdventure.world.chunks.GetLength(0) && chunkY >= 0 && chunkY < ConsoleAdventure.world.chunks.GetLength(1))
            {
                Chunk chunk = chunks[chunkX, chunkY];

                if (chunk != null) 
                {
                    if (chunk is UnloadedChunk)
                    {

                        LoadChunk(chunkX, chunkY, false);

                        if (lastLoadedChunk.HasValue && (lastLoadedChunk.Value != new Position(chunkX, chunkY)))
                        {
                            UnloadChunk(lastLoadedChunk.Value.x, lastLoadedChunk.Value.y);
                        }

                        if (chunks[chunkX, chunkY] is LoadedChunk)
                            lastLoadedChunk = new(chunkX, chunkY);
                    }

                    Field field = chunks[chunkX, chunkY].GetField(localX, localY, layer, w);
                    return field;
                }
            }

            return new();
        }

        public short GetFieldTypeAnyway(int x, int y, int layer, int w)
        {
            Chunk chunk = GetChunk(x, y, out int localX, out int localY);
            if (chunk != null)
            {
                if (chunk is UnloadedChunk)
                    return ((UnloadedChunk)chunk).fields[localX, localY, layer, w];

                else if (chunk is LoadedChunk)
                {
                    short? type = chunk.GetField(localX, localY, layer, w)?.content?.type;

                    if (!type.HasValue) type = 0;

                    return type.Value;
                }
            }

            return 0;
        }

        public UnloadedChunk GetUnloadedChunk(int x, int y, out int localX, out int localY)
        {
            Chunk chunk = GetChunk(x, y, out localX, out localY);

            if (chunk != null && chunk is UnloadedChunk)
                return (UnloadedChunk)chunk;

            return null;
        }

        public Chunk GetChunk(int x, int y, out int localX, out int localY)
        {
            int chunkX = x / Chunk.Size;
            int chunkY = y / Chunk.Size;
            localX = x % Chunk.Size;
            localY = y % Chunk.Size;

            if (ConsoleAdventure.world?.chunks == null) return null;
            if (chunkX >= 0 && chunkX < ConsoleAdventure.world.chunks.GetLength(0) && chunkY >= 0 && chunkY < ConsoleAdventure.world.chunks.GetLength(1))
            {
                return chunks[chunkX, chunkY];
            }

            return null;
        }

        public void SetFieldTypeAnyway(int x, int y, int layer, int w, short type)
        {
            Chunk chunk = GetChunk(x, y, out int localX, out int localY);
            if (chunk != null)
            {
                if (chunk is UnloadedChunk)
                    ((UnloadedChunk)chunk).fields[localX, localY, layer, w] = type;

                else if (chunk is LoadedChunk)
                    Transform.SetObject(type, new Position(x, y), w, layer);
            }
        }

        public void SetFieldDataAnyway(TransformDataInChunk data)
        {
            int x = data.position.x;
            int y = data.position.y;

            Chunk chunk = GetChunk(x, y, out int localX, out int localY);
            if (chunk != null)
            {
                if (chunk is UnloadedChunk)
                    ((UnloadedChunk)chunk).data.Add(data);

                else if (chunk is LoadedChunk)
                {
                    Transform transform = GetField(x, y, data.z, data.w)?.content;

                    if (transform != null)
                    {
                        transform.LoadData(data.data);
                    }
                }
            }
        }

        public Field GetField(Position position, int layer, int w)
        {
            return GetField(position.x, position.y, layer, w);
        }

        public Transform GetTransform(int x, int y, int layer, int w)
        {
            return GetField(x, y, layer, w)?.content;
        }

        public Transform GetTransform(Position position, int layer, int w)
        {
            return GetField(position.x, position.y, layer, w)?.content;
        }

        /*public void SetField(int x, int y, int layer, int w, Field field)
        {
            int chunkX = x / Chunk.Size;
            int chunkY = y / Chunk.Size;
            int localX = x % Chunk.Size;
            int localY = y % Chunk.Size;

            if (chunkX >= 0 && chunkX < ConsoleAdventure.world.chunks.GetLength(0) && chunkY >= 0 && chunkY < ConsoleAdventure.world.chunks.GetLength(1))
            {
                chunks[chunkX, chunkY].SetField(localX, localY, layer, w, field);
            }
        }*/

        public Chunk[,] GetChunks()
        {
            return chunks;
        }

        public void InitializeChunks()
        {
            int chunkCount = (size + Chunk.Size - 1) / Chunk.Size;

            chunks = new Chunk[chunkCount, chunkCount];

            /*
            for (int x = 0; x < chunkCount; x++)
            {
                for (int y = 0; y < chunkCount; y++)
                {
                    chunks[x, y] = new UnloadedChunk();
                }
            }
            */
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

        public void LoadChunk(int xChunk, int yChunk, bool playerArea)
        {
            //ChunkManager.RequestLoad(new(xChunk, yChunk));
            ChunkManager.LoadChunk(xChunk, yChunk, playerArea);
        }

        public void UnloadChunk(int xChunk, int yChunk)
        {
            //ChunkManager.RequestUnload(new(xChunk, yChunk));
            ChunkManager.UnloadChunk(xChunk, yChunk);
        }

        internal void UnloadAllChunks()
        {
            if (chunks == null) return;

            for (int i = 0; i < chunks.GetLength(0); i++)
            {
                for (int j = 0; j < chunks.GetLength(1); j++)
                {
                    Chunk chunk = chunks[i, j];

                    if(chunk is LoadedChunk)
                    {
                        UnloadChunk(i, j);
                    }
                }
            }
        }

        public async Task GenerateChunk(int x, int y)
        {
            chunks[x, y] = new LoadedChunk();

            await generator.Generate(this, new Position(x, y));

            if (chunks[x, y] != null) 
                chunks[x, y].IsUpdated = false;
        }
    }
}
