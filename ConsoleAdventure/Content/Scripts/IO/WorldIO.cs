using CaModLoaderAPI;
using ConsoleAdventure.CaModLoaderAPI;
using ConsoleAdventure.Content.Scripts.Player;
using ConsoleAdventure.Content.Scripts.Settings;
using ConsoleAdventure.Content.Scripts.UI;
using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using ConsoleAdventure.WorldEngine.Levels;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Xml.Linq;

namespace ConsoleAdventure.Content.Scripts.IO
{
    internal class WorldIO
    {

        private static readonly object locker = new object();
        private static string path = Program.savePath + "Worlds\\";

        public static void Save(string name)
        {
            if (!NetworkManager.isHost && ConsoleAdventure.world.inMultiplayer) return;
            lock (locker)
            {
                ConsoleAdventure.logger.AddMessage($"The world {name} saving...");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                byte[] bytes = GetWorldBytes();

                string fileName = path + name + ".wld";

                if (bytes != null)
                {
                    File.WriteAllBytes(fileName, bytes);
                }
                else
                {
                    ConsoleAdventure.logger.AddMessage("There was a problem when serializing tags (the byte array cannot be null)");
                    return;
                }

                ConsoleAdventure.logger.AddMessage("The world was successfully saved!");
            }
        }

        public static byte[] GetWorldBytes()
        {
            CreateTags(); //Создаём теги и храним в них данные о мире
            byte[] bytes = SerializeData.Serialize(ConsoleAdventure.tags.Data); //Переводим теги в массив байтов
            byte[] bytesToSave = Utils.Compress(bytes, CompressionLevel.SmallestSize);
            return bytesToSave;
        }

        public static void Load(string name)
        {
            lock (locker)
            {
                ConsoleAdventure.progressBar.stepText = Localization.GetTranslation("Progress", "LoadFile");

                ConsoleAdventure.logger.AddMessage($"The world {name} loading...");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = path + name + ".wld";

                if (File.Exists(fileName))
                {
                    byte[] bytes = File.ReadAllBytes(fileName);
                    SetWorldFromBytes(bytes);
                }

                else
                {
                    ConsoleAdventure.logger.AddMessage($"The world {name} was not found");
                    return;
                }

                ConsoleAdventure.progressBar.Progress = 5;

                ConsoleAdventure.world.name = name;
                LoadTags(); //Загружам данные из тегов в мир

                ConsoleAdventure.world.Loaded();

                ConsoleAdventure.logger.AddMessage("The world was successfully loaded!");
            }
        }

        public static void LoadWorldFromPackedBytes(byte[] worldBytes)
        {
            lock (locker)
            {
                ConsoleAdventure.progressBar.stepText = Localization.GetTranslation("MProgress", "LoadingWorld");

                ConsoleAdventure.logger.AddMessage($"The received world loading...");

                SetWorldFromBytes(worldBytes);

                ConsoleAdventure.progressBar.Progress = 5;

                ConsoleAdventure.world.name = "reveivedWorld";
                LoadTags(); //Загружам данные из тегов в мир

                ConsoleAdventure.world.Loaded();

                ConsoleAdventure.logger.AddMessage("The received world was successfully loaded!");
            }
        }

        public static void SetWorldFromBytes(byte[] bytes)
        {
            byte[] bytesToLoad = Utils.Decompress(bytes);
            ConsoleAdventure.tags.Data = SerializeData.Deserialize<Dictionary<string, object>>(bytesToLoad); //Переводим байты в теги
        }

        public static void Delete(string name)
        {
            string worldPath = path + name + ".wld";
            ConsoleAdventure.logger.AddMessage($"The world {name} deleting...");
            try
            {
                if (File.Exists(worldPath))
                {
                    FileSystem.DeleteFile(worldPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    ConsoleAdventure.logger.AddMessage("File moved to recycle bin successfully.");
                }
                else
                {
                    ConsoleAdventure.logger.AddMessage("File not found.");
                }
            }
            catch (Exception ex)
            {
                ConsoleAdventure.logger.AddMessage($"An error occurred: {ex.Message}");
            }
        }

        public static (string[] names, int[] seeds) GetWorlds(bool getSeed = true, bool print = true)
        {
            string fileType = "*.wld";

            string[] names = new string[1];
            int[] seeds = new int[1];

            names = Directory.GetFiles(path, fileType);
            seeds = new int[names.Length];

            StringBuilder sb = new();

            for (int i = 0; i < names.Length; i++)
            {
                try
                {
                    names[i] = Path.GetFileNameWithoutExtension(names[i]);
                    if (getSeed)
                    {
                        byte[] bytes = File.ReadAllBytes(path + names[i] + ".wld");
                        byte[] bytesToLoad = Utils.Decompress(bytes);
                        Tags curTags = new();
                        curTags.Data = SerializeData.Deserialize<Dictionary<string, object>>(bytesToLoad);
                        seeds[i] = curTags.SafelyGet<int>("Seed");
                    }

                    if (print)
                        sb.Append("\n    [" + names[i] + " / " + seeds[i] + "]");
                }
                catch (Exception ex)
                {
                    ConsoleAdventure.logger.AddMessage($"An error occurred: {ex.Message}");
                }
            }

            ConsoleAdventure.logger.AddMessage($"Found {names.Length} world(s):{sb}\n");

            return (names, seeds);
        }

        public static void CreateTags()
        {
            World world = ConsoleAdventure.world;
            Tags tags = new();
            int size = world.size;
            int deep = Chunk.maxDeep;

            CaModLoader.PreSaveWorldMods();

            //world.UnloadAllChunks();

            tags["Seed"] = world.seed;

            tags["Size"] = size;
            tags["Deep"] = deep;
            tags["ChunksWidth"] = world.GetChunkCounts().X; 
            tags["ChunksHeight"] = world.GetChunkCounts().Y;

            tags["Time"] = world.time;

            tags["ModTransforms"] = world.modTransforms;
            tags["VanillaTransforms"] = Main.vanillaTypesInitialized;

            List<WorldLevel> rawLevels = world.levels.Levels;
            List<string> levels = new();

            for (int i = 0; i < rawLevels.Count; i++)
            {
                if (rawLevels[i] is UnloadWorldLevel)
                    levels.Add(((UnloadWorldLevel)rawLevels[i]).Name);

                else levels.Add(rawLevels[i].GetType().FullName);
            }

            tags["WorldLevels"] = levels.ToArray();

            Dictionary<string, byte[]> playersData = new Dictionary<string, byte[]>();

            short[] players = new short[ConsoleAdventure.world.players.Count];
            ConsoleAdventure.world.players.Keys.CopyTo(players, 0);

            for (int i = 0; i < players.Length; i++)
            {
                Player.Player player = ConsoleAdventure.world.players[players[i]];
                if (!playersData.ContainsKey(player.info.pcId))
                    playersData.Add(player.info.pcId, player.GetPlayerBytes());
            }

            tags["PlayersData"] = playersData;

            List<Position> chunksPositions = new List<Position>();

            for (int x = 0; x < world.GetChunkCounts().X; x++)
            {
                for (int y = 0; y < world.GetChunkCounts().Y; y++)
                {
                    Chunk chunk = world.chunks[x, y];

                    if (chunk != null)
                    {
                        if (chunk.IsUpdated)
                        {
                            chunksPositions.Add(new Position(x, y));
                        }
                    }
                }
            }

            byte[,,,,] fields = new byte[chunksPositions.Count ,deep, Chunk.Size, Chunk.Size, 3]; //c, w, x, y, z

            for (int c = 0; c < chunksPositions.Count; c++)
            {
                for (int w = 0; w < deep; w++)
                {
                    for (int x = 0; x < Chunk.Size; x++)
                    {
                        for (int y = 0; y < Chunk.Size; y++)
                        {
                            for (int z = 0; z < 3; z++)
                            {
                                int X = (chunksPositions[c].x * Chunk.Size) + x;
                                int Y = (chunksPositions[c].y * Chunk.Size) + y;

                                fields[c, w, x, y, z] = (byte)world.GetFieldTypeAnyway(X, Y, z, w);
                            }
                        }
                    }
                }
            }

            List<object> transformsData = new();
            List<int> transformsDataX = new();
            List<int> transformsDataY = new();
            List<byte> transformsDataZ = new();
            List<byte> transformsDataW = new();

            for (int i = 0; i < world.chunks.GetLength(0); i++)
            {
                for (int j = 0; j < world.chunks.GetLength(1); j++)
                {
                    Chunk chunk = world.chunks[i, j];
                    if (chunk is UnloadedChunk)
                    {
                        List<TransformDataInChunk> data = ((UnloadedChunk)world.chunks[i, j]).data;

                        for (int k = 0; k < data.Count; k++)
                        {
                            transformsDataX.Add(data[k].position.x);
                            transformsDataY.Add(data[k].position.y);
                            transformsDataZ.Add(data[k].z);
                            transformsDataW.Add(data[k].w);
                            transformsData.Add(data[k].data);
                        }
                    }

                    else if (chunk is LoadedChunk)
                    { 
                        for (int k = 0; k < Chunk.maxDeep; k++)
                        {
                            for (int l = 0; l < Chunk.Size; l++)
                            {
                                for (int m = 0; m < Chunk.Size; m++)
                                {
                                    for (int n = 0; n < 3; n++)
                                    {
                                        Field field = chunk.GetField(l, m, n, k);

                                        if (field?.content != null)
                                        {

                                            transformsDataX.Add(field.content.position.x);
                                            transformsDataY.Add(field.content.position.y);
                                            transformsDataZ.Add(field.content.worldLayer);
                                            transformsDataW.Add(field.content.w);
                                            transformsData.Add(field.content.SaveData());
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            tags["Fields"] = fields;
            tags["ChunksPositions"] = chunksPositions.ToArray();

            tags["TransformsData"] = transformsData.ToArray();
            tags["TransformsDataX"] = transformsDataX.ToArray();
            tags["TransformsDataY"] = transformsDataY.ToArray();
            tags["TransformsDataZ"] = transformsDataZ.ToArray();
            tags["TransformsDataW"] = transformsDataW.ToArray();

            int EntityCount = world.entities.Count;
            int[] EntityX = new int[EntityCount];
            int[] EntityY = new int[EntityCount];
            int[] EntityW = new int[EntityCount];
            byte[] EntityTypes = new byte[EntityCount];
            List<object>[] EntityParams = new List<object>[EntityCount];

            for (int i = 0; i < EntityCount; i++)
            {
                Entity entity = world.entities[i];

                EntityW[i] = entity.w;
                EntityX[i] = entity.position.x;
                EntityY[i] = entity.position.y;
                EntityTypes[i] = entity.type;
                EntityParams[i] = entity.GetParams();
            }

            tags["EntityCount"] = EntityCount;
            tags["EntityX"] = EntityX;
            tags["EntityY"] = EntityY;
            tags["EntityW"] = EntityW;
            tags["EntityTypes"] = EntityTypes;
            tags["EntityParams"] = EntityParams;

            CaModLoader.SaveModTagsMods(tags);

            ConsoleAdventure.tags = tags;
        }

        public static void InitContent()
        {
            Transform.ClearTypeMap();

            Light.Clear();

            Transform.IsGlobalInit = true;

            Type baseType = typeof(Transform);
            IEnumerable<Type> list = Assembly.GetAssembly(baseType).GetTypes().Where(type => type.IsSubclassOf(baseType)).ToList().Concat(CaModLoader.modTransforms);
            foreach (Type type in list)
            {
                if (type.IsAbstract)
                    continue;

                Transform.Init(type, Position.Zero(), 0, null, null);
            }

            Transform.IsGlobalInit = false;

            ConsoleAdventure.recipes.Clear();

            IEnumerable<Type> listItem = Assembly.GetAssembly(typeof(Item)).GetTypes().Where(type => type.IsSubclassOf(typeof(Item))).ToList().Concat(CaModLoader.modItems);

            foreach (Type type in listItem)
            {
                if (type.IsAbstract)
                    continue;

                Item item = (Item)Activator.CreateInstance(type);
                Recipe recipe = item.AddRecipe();

                bool isEdited = false;
                foreach (GlobalItem globalItem in CaModLoader.modGlobalItems)
                {
                    Recipe editRecipe = globalItem.EditRecipes(item);
                    if (editRecipe != null)
                    {
                        if (!isEdited)
                        {
                            recipe = editRecipe;
                            isEdited = true;
                        }

                        else ConsoleAdventure.recipes.Add(editRecipe);
                    }               
                }

                if (recipe != null)
                    ConsoleAdventure.recipes.Add(recipe);
            }

            CaModLoader.PostInitContentMods();
        }

        public static void LoadTags()
        {
            lock (locker)
            {
                Loger.AddLog("start load");
                ConsoleAdventure.progressBar.stepText = Localization.GetTranslation("Progress", "LoadGenericData");

                Display.recipesUI = new RecipesUI(new Point(ConsoleAdventure.screenWidth / 2, ConsoleAdventure.screenHeight / 2), new Point(60, 15));

                World world = ConsoleAdventure.world;
                Tags tags = ConsoleAdventure.tags;

                CaModLoader.PreLoadWorldMods();

                world.seed = tags.SafelyGet<int>("Seed");
                world.size = tags.SafelyGet<int>("Size");
                world.time = tags.SafelyGet<Time>("Time");

                int lastVanillaTransformCount = tags.SafelyGet<int>("VanillaTransforms");
                world.modTransforms = tags.SafelyGet<Dictionary<string, int>>("ModTransforms");

                int newVanillaTransformsCount = Main.vanillaTypesInitialized - lastVanillaTransformCount;

                if (newVanillaTransformsCount != 0)
                {
                    foreach (var key in world.modTransforms.Keys.ToList())
                    {
                        world.modTransforms[key] += newVanillaTransformsCount;
                    }
                }

                Main.InitTransformsTypes(0);
                InitContent();

                string[] levels = tags.SafelyGet<string[]>("WorldLevels", null);
                List<WorldLevel> worldLevels = new();

                if (levels != null)
                {
                    foreach (var level in levels)
                    {
                        Type type = Type.GetType(level);

                        if (type == null)
                        {
                            foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
                            {
                                type = asm.GetType(level);
                                if (type != null) break;
                            }
                        }

                        if (type == null)
                        {
                            UnloadWorldLevel unloadWorldLevel = new UnloadWorldLevel();
                            unloadWorldLevel.Name = level;

                            worldLevels.Add(unloadWorldLevel);
                            continue;
                        }

                        object createdLevel = Activator.CreateInstance(type);

                        if (createdLevel.GetType().IsSubclassOf(typeof(WorldLevel)))
                        {
                            worldLevels.Add((WorldLevel)createdLevel);
                        }
                    }
                }

                world.levels = new WorldLevelsSystem(worldLevels);
                world.SetDeeps();

                world.InitializeChunks();

                ConsoleAdventure.world.playersDat = tags.SafelyGet<Dictionary<string, byte[]>>("PlayersData");

                byte[,,,,] fields = tags.SafelyGet<byte[,,,,]>("Fields");
                Position[] chunks = tags.SafelyGet<Position[]>("ChunksPositions");

                ConsoleAdventure.progressBar.Progress += 10;
                ConsoleAdventure.progressBar.stepText = Localization.GetTranslation("Progress", "LoadObjectData");

                int size = world.size;
                int deep = Chunk.maxDeep;

                for (int c = 0; c < chunks.Length; c++)
                {
                    Position chunkPos = chunks[c];

                    world.chunks[chunkPos.x, chunkPos.y] = new UnloadedChunk() { IsUpdated = true };

                    for (int w = 0; w < deep; w++)
                    {
                        for (int x = 0; x < Chunk.Size; x++)
                        {
                            for (int y = 0; y < Chunk.Size; y++)
                            {
                                for (int z = 0; z < 3; z++)
                                {
                                    int X = (chunkPos.x * Chunk.Size) + x;
                                    int Y = (chunkPos.y * Chunk.Size) + y;

                                    byte type = fields[c, w, x, y, z];

                                    if (type > lastVanillaTransformCount - 1)
                                        type += (byte)newVanillaTransformsCount;

                                    world.SetFieldTypeAnyway(X, Y, z, w, type);
                                }
                            }
                        }
                    }
                }

                object[] transformsData = tags.SafelyGet<object[]>("TransformsData");
                int[] transformsDataX = tags.SafelyGet<int[]>("TransformsDataX");
                int[] transformsDataY = tags.SafelyGet<int[]>("TransformsDataY");
                byte[] transformsDataZ = tags.SafelyGet<byte[]>("TransformsDataZ");
                byte[] transformsDataW = tags.SafelyGet<byte[]>("TransformsDataW");

                if (transformsData != null)
                {
                    for (int i = 0; i < transformsData.Length; i++)
                    {
                        world.SetFieldDataAnyway(new((short)transformsDataX[i], 
                                                     (short)transformsDataY[i], 
                                                     transformsDataZ[i], 
                                                     transformsDataW[i], 
                                                     transformsData[i]));
                    }
                }

                ConsoleAdventure.progressBar.Progress += 30;
                ConsoleAdventure.progressBar.stepText = Localization.GetTranslation("Progress", "LoadEntityData");

                int EntityCount = tags.SafelyGet<int>("EntityCount");
                int[] EntityX = tags.SafelyGet<int[]>("EntityX");
                int[] EntityY = tags.SafelyGet<int[]>("EntityY");
                int[] EntityW = tags.SafelyGet<int[]>("EntityW");
                byte[] EntityTypes = tags.SafelyGet<byte[]>("EntityTypes");
                List<object>[] EntityParams = tags.SafelyGet<List<object>[]>("EntityParams");

                for (int i = 0; i < world.entities.Count; i++)
                {
                    world.entities[i].Kill();
                }

                world.entities.Clear();

                for (int i = 0; i < EntityCount; i++)
                {
                    if (EntityX[i] == 5 && EntityY[i] == 5)
                    {
                        EntityX[i] = 0;
                        EntityY[i] = 0;
                    }

                    int type = EntityTypes[i];
                    if (type > lastVanillaTransformCount - 1)
                        type += (byte)newVanillaTransformsCount;

                    Transform.SetObject(type, new Position(EntityX[i], EntityY[i]), EntityW[i], parameters: EntityParams[i]);
                }

                CaModLoader.LoadModTagsMods(tags);

                ConsoleAdventure.progressBar.Progress += 5;
                ConsoleAdventure.InWorld = true;

                Loger.AddLog("end load");
            }
        }
    }
}
