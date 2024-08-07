﻿using CaModLoaderAPI;
using ConsoleAdventure.WorldEngine;
using Microsoft.VisualBasic.FileIO;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;
using System.Runtime.ExceptionServices;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace ConsoleAdventure.Content.Scripts.IO
{
    internal class WorldIO
    {

        private static readonly object locker = new object();
        private static string path = Program.savePath + "Worlds\\";

        public static void Save(string name)
        {
            Console.WriteLine("Saving on account");

            CreateTags(); //Создаём теги и храним в них данные о мире

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            byte[] bytes = SerializeData.Serialize(ConsoleAdventure.tags.Data); //Переводим теги в массив байтов
            byte[] bytesToSave = Utils.Compress(bytes, CompressionLevel.SmallestSize);

            string fileName = path + name + ".wld";

            if (bytes != null)
            {
                File.WriteAllBytes(fileName, bytesToSave);
            }
            else
            {
                Console.WriteLine("There was a problem when serializing tags (the byte array cannot be null)");
                return;
            }

            Console.WriteLine("The world was successfully saved!");
        }

        public static void Load(string name)
        {
            lock (locker)
            {
                ConsoleAdventure.progressBar.stepText = Localization.GetTranslation("Progress", "LoadFile");

                Console.WriteLine("Loading on account");

                if (!Directory.Exists(path))
                {
                    Directory.CreateDirectory(path);
                }

                string fileName = path + name + ".wld";

                if (File.Exists(fileName))
                {
                    byte[] bytes = File.ReadAllBytes(fileName);
                    byte[] bytesToLoad = Utils.Decompress(bytes);

                    ConsoleAdventure.tags.Data = SerializeData.Deserialize<Dictionary<string, object>>(bytesToLoad); //Переводим байты в теги
                }

                else
                {
                    Console.WriteLine($"The world {name} was not found");
                    return;
                }

                ConsoleAdventure.progressBar.Progress = 5;

                ConsoleAdventure.world.name = name;
                LoadTags(); //Загружам данные из тегов в мир

                Console.WriteLine("The world was successfully loaded!");
            }
        }

        public static void Delete(string name)
        {
            string worldPath = path + name + ".wld";
            try
            {
                if (File.Exists(worldPath))
                {
                    FileSystem.DeleteFile(worldPath, UIOption.OnlyErrorDialogs, RecycleOption.SendToRecycleBin);
                    Console.WriteLine("File moved to recycle bin successfully.");
                }
                else
                {
                    Console.WriteLine("File not found.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }
        }

        public static (string[] names, int[] seeds) GetWorlds()
        {
            string fileType = "*.wld";

            string[] names = new string[1];
            int[] seeds = new int[1];

            try
            {
                names = Directory.GetFiles(path, fileType);
                seeds = new int[names.Length];

                Console.WriteLine($"Found {names.Length} file(s) with extension {fileType}:");

                for (int i = 0; i < names.Length; i++) 
                {
                    names[i] = Path.GetFileNameWithoutExtension(names[i]);

                    Console.WriteLine(names[i]);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

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

        private static void LoadTags()
        {
            lock (locker)
            {
                ConsoleAdventure.progressBar.stepText = Localization.GetTranslation("Progress", "LoadGenericData");

                Type baseType = typeof(Transform);
                IEnumerable<Type> list = Assembly.GetAssembly(baseType).GetTypes().Where(type => type.IsSubclassOf(baseType));
                foreach (Type type in list)
                {
                    Transform.Init(type, Position.Zero(), 0, null, null);
                }

                World world = ConsoleAdventure.world;
                Tags tags = ConsoleAdventure.tags;

                world.seed = tags.SafelyGet<int>("Seed");

                world.size = tags.SafelyGet<int>("Size");
                //world.seed = (int)tags.SafelyGet("Seed");
                //world.seed = (int)tags.SafelyGet("Seed");

                world.time = tags.SafelyGet<Time>("Time");

                Main.modTransformTypesOffset = tags.SafelyGet<Dictionary<string, byte>>("TransformTypesOffset");

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
