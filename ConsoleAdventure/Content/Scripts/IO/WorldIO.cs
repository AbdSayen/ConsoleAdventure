using CaModLoaderAPI;
using ConsoleAdventure.CaModLoaderAPI;
using ConsoleAdventure.Content.Scripts.Player;
using ConsoleAdventure.Content.Scripts.UI;
using ConsoleAdventure.WorldEngine;
using Microsoft.VisualBasic.FileIO;
using Microsoft.Xna.Framework;
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

        private static void CreateTags()
        {
            World world = ConsoleAdventure.world;
            Tags tags = new();
            int size = world.size;

            tags["Seed"] = world.seed;

            tags["Size"] = size;
            tags["ChunksX"] = world.GetChunkCounts().X;
            tags["ChunksY"] = world.GetChunkCounts().Y;

            tags["Time"] = world.time;

            tags["TransformTypesOffset"] = Main.modTransformTypesOffset;

            Dictionary<string, byte[]> playersDat = new Dictionary<string, byte[]>();


            short[] players = new short[ConsoleAdventure.world.players.Count];
            ConsoleAdventure.world.players.Keys.CopyTo(players, 0);

            for (int i = 0; i < players.Length; i++)
            {
                Player.Player player = ConsoleAdventure.world.players[players[i]];
                if (!playersDat.ContainsKey(player.info.pcId))
                    playersDat.Add(player.info.pcId, player.GetPlayerBytes());
            }

            tags["PlayersData"] = playersDat;

            byte[,,,] fields = new byte[Chunk.maxDeep, size, size, 4]; //w, x, y, z


            int lootCount = 0;
            for (int i = 0; i < Chunk.maxDeep; i++) //w, поик количества лута и сундуков
            {
                for (int j = 0; j < size; j++) //x
                {
                    for (int k = 0; k < size; k++) //y
                    {
                        if (world.GetField(j, k, World.ItemsLayerId, i).content != null)
                        {
                            lootCount++;
                        }
                    }
                }
            } 
            
            List<Stack>[] loots = new List<Stack>[lootCount];
            int[] lootX = new int[lootCount];
            int[] lootY = new int[lootCount];
            int[] lootW = new int[lootCount];
            byte[] lootTypes = new byte[lootCount]; //Тип объекта: лут или один из сундуков

            int curLoot = 0;

            for (int i = 0; i < Chunk.maxDeep; i++) //w
            {
                for (int j = 0; j < size; j++) //x
                {
                    for (int k = 0; k < size; k++) //y
                    {
                        for (int l = 0; l < 4; l++) //z
                        {
                            byte type = 0;
                            if (world.GetField(j, k, l, i).content != null) //Поиск ячеяк мира
                            {
                                type = (byte)world.GetField(j, k, l, i).content.type;
                            }

                            fields[i, j, k, l] = type;
                        }

                        Field field = world.GetField(j, k, World.ItemsLayerId, i);

                        if (field.content != null) //Поиск лута и сундуков
                        {
                            loots[curLoot] = ((Storage)field.content).GetItems();
                            lootW[curLoot] = i;
                            lootX[curLoot] = j;
                            lootY[curLoot] = k;
                            lootTypes[curLoot] = field.content.type;
                            curLoot++;
                        }
                    }
                }
            }

            tags["Fields"] = fields;

            tags["LootCount"] = lootCount;
            tags["Loots"] = loots;
            tags["LootX"] = lootX;
            tags["LootY"] = lootY;
            tags["LootW"] = lootW;

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

            ConsoleAdventure.tags = tags;
        }

        public static void LoadTags()
        {
            lock (locker)
            {
                ConsoleAdventure.progressBar.stepText = Localization.GetTranslation("Progress", "LoadGenericData");

                Light.Clear();

                Type baseType = typeof(Transform);
                IEnumerable<Type> list = Assembly.GetAssembly(baseType).GetTypes().Where(type => type.IsSubclassOf(baseType));
                foreach (Type type in list)
                {
                    Transform.Init(type, Position.Zero(), 0, null, null);
                }

                ConsoleAdventure.recipes.Clear();

                IEnumerable<Type> listItem = Assembly.GetAssembly(typeof(Item)).GetTypes().Where(type => type.IsSubclassOf(typeof(Item)));
                foreach (Type type in listItem)
                {
                    if (type.IsAbstract)
                        continue;

                    Item item = (Item)Activator.CreateInstance(type);
                    Recipe recipe = item.AddRecipe();
                    if (recipe != null)
                        ConsoleAdventure.recipes.Add(recipe);
                }

                Display.recipesUI = new RecipesUI(new Point(ConsoleAdventure.screenWidth / 2, ConsoleAdventure.screenHeight / 2), new Point(60, 15));

                World world = ConsoleAdventure.world;
                Tags tags = ConsoleAdventure.tags;

                world.seed = tags.SafelyGet<int>("Seed");

                world.size = tags.SafelyGet<int>("Size");
                //world.seed = (int)tags.SafelyGet("Seed");
                //world.seed = (int)tags.SafelyGet("Seed");

                world.time = tags.SafelyGet<Time>("Time");

                Main.modTransformTypesOffset = tags.SafelyGet<Dictionary<string, byte>>("TransformTypesOffset");

                ConsoleAdventure.world.playersDat = tags.SafelyGet<Dictionary<string, byte[]>>("PlayersData");

                byte[,,,] fields = tags.SafelyGet<byte[,,,]>("Fields");

                ConsoleAdventure.progressBar.Progress += 10;
                ConsoleAdventure.progressBar.stepText = Localization.GetTranslation("Progress", "LoadObjectData");

                for (int i = 0; i < Chunk.maxDeep; i++) //w
                {
                    for (int j = 0; j < world.size; j++) //x
                    {
                        for (int k = 0; k < world.size; k++) //y
                        {
                            for (int l = 0; l < 4; l++) //z
                            {
                                if (l != World.MobsLayerId)
                                {
                                    byte type = fields[i, j, k, l];
                                    if (type != (byte)RenderFieldType.loot && type != (byte)RenderFieldType.chest)
                                    {
                                        Transform.SetObject(type, new(j, k), i, l); //загружаем ячейки из тега
                                    }

                                    else
                                    {
                                        List<Stack> items = new List<Stack>();

                                        for (int m = 0; m < tags.SafelyGet<int>("LootCount"); m++)
                                        {
                                            Position position = new Position((tags.SafelyGet<int[]>("LootX"))[m], (tags.SafelyGet<int[]>("LootY"))[m]);
                                            int w = tags.SafelyGet<int[]>("LootW")[m];

                                            if (w == i && position.x == j && position.y == k)
                                            {
                                                items = (tags.SafelyGet<List<Stack>[]>("Loots"))[m];
                                                break;
                                            }
                                        }

                                        Transform.SetObject(type, new(j, k), i, items: items); //Загружам лут из мира
                                    }
                                }
                            }
                        }
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

                    Transform.SetObject(EntityTypes[i], new Position(EntityX[i], EntityY[i]), EntityW[i], parameters: EntityParams[i]);
                }

                ConsoleAdventure.progressBar.Progress += 5;
                ConsoleAdventure.InWorld = true;
            }
        }
    }
}
