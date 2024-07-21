using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;

namespace ConsoleAdventure.Content.Scripts.IO
{
    internal class Saves
    {
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

            string fileName = path + name + ".wld";

            if (bytes != null)
            {
                File.WriteAllBytes(fileName, bytes);
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
            Console.WriteLine("Loading on account");

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            string fileName = path + name + ".wld";

            if (File.Exists(fileName))
            {
                byte[] bytes = File.ReadAllBytes(fileName);

                ConsoleAdventure.tags.Data = SerializeData.Deserialize<Dictionary<string, object>>(bytes); //Переводим байты в теги
            }
            else
            {
                Console.WriteLine($"The world {name} was not found");
                return;
            }

            LoadTags(); //Загружам данные из тегов в мир

            Console.WriteLine("The world was successfully loaded!");
        }

        private static void CreateTags()
        {
            World world = ConsoleAdventure.world;
            Tags tags = new();
            int size = world.size;

            tags["Seed"] = world.seed;

            tags["Size"] = size;
            tags["ChunksX"] = world.GetCunkCounts().X;
            tags["ChunksY"] = world.GetCunkCounts().Y;

            tags["Time"] = world.time;

            byte[,,] fields = new byte[size, size, 4];


            int lootCount = 0;
            for (int i = 0; i < size; i++) //поик количества лута
            {
                for (int j = 0; j < size; j++)
                {
                    if (world.GetField(i, j, World.ItemsLayerId).content != null)
                    {
                        lootCount++;
                    }
                }
            } 
            
            List<Stack>[] loots = new List<Stack>[lootCount];
            int[] lootX = new int[lootCount];
            int[] lootY = new int[lootCount];

            int curLoot = 0;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        byte type = 0;
                        if (world.GetField(i, j, k).content != null) //Поиск ячеяк мира
                        {
                            type = (byte)world.GetField(i, j, k).content.renderFieldType;
                        }

                        fields[i, j, k] = type;
                    }

                    if (world.GetField(i, j, World.ItemsLayerId).content != null) //Поиск лута
                    {
                        loots[curLoot] = ((Loot)world.GetField(i, j, World.ItemsLayerId).content).GetItems();
                        lootX[curLoot] = i;
                        lootY[curLoot] = j;
                        curLoot++;
                    }
                }
            }

            tags["Fields"] = fields;

            tags["LootCount"] = lootCount;
            tags["Loots"] = loots;
            tags["LootX"] = lootX;
            tags["LootY"] = lootY;

            int EntityCount = world.entitys.Count;
            int[] EntityX = new int[EntityCount];
            int[] EntityY = new int[EntityCount];
            byte[] EntityTypes = new byte[EntityCount];
            List<object>[] EntityParams = new List<object>[EntityCount];

            for (int i = 0; i < EntityCount; i++)
            {
                Entity entity = world.entitys[i];

                EntityX[i] = entity.position.x;
                EntityY[i] = entity.position.y;
                EntityTypes[i] = (byte)entity.renderFieldType;
                EntityParams[i] = entity.GetParams();
            }

            tags["EntityCount"] = EntityCount;
            tags["EntityX"] = EntityX;
            tags["EntityY"] = EntityY;
            tags["EntityTypes"] = EntityTypes;
            tags["EntityParams"] = EntityParams;

            ConsoleAdventure.tags = tags;
        }

        private static void LoadTags()
        {
            World world = ConsoleAdventure.world;
            Tags tags = ConsoleAdventure.tags;

            world.seed = (int)tags.SafelyGet("Seed");

            world.size = (int)tags.SafelyGet("Size");
            //world.seed = (int)tags.SafelyGet("Seed");
            //world.seed = (int)tags.SafelyGet("Seed");

            world.time = (Time)tags.SafelyGet("Time");

            for (int i = 0; i < world.size; i++)
            {
                for (int j = 0; j < world.size; j++)
                {
                    for (int k = 0; k < 4; k++)
                    {
                        if (k != World.MobsLayerId)
                        {
                            byte type = ((byte[,,])tags.SafelyGet("Fields"))[i, j, k];
                            if (type != (byte)RenderFieldType.loot)
                            {
                                Transform.SetObject(type, new(i, j), k); //загружаем ячейки из тега
                            }

                            else
                            {
                                List<Stack> items = new List<Stack>();

                                for (int l = 0; l < (int)tags.SafelyGet("LootCount"); l++)
                                {
                                    Position position = new Position(((int[])tags.SafelyGet("LootX"))[l], ((int[])tags.SafelyGet("LootY"))[l]);
                                    if (position.x == i && position.y == j)
                                    {
                                        items = ((List<Stack>[])tags.SafelyGet("Loots"))[l];
                                        break;
                                    }
                                }

                                Transform.SetObject(type, new(i, j), items: items); //Загружам лут из мира
                            }
                        }
                    }
                }
            }

            int EntityCount = (int)tags.SafelyGet("EntityCount");
            int[] EntityX = (int[])tags.SafelyGet("EntityX");
            int[] EntityY = (int[])tags.SafelyGet("EntityY");
            byte[] EntityTypes = (byte[])tags.SafelyGet("EntityTypes");
            List<object>[] EntityParams = (List<object>[])tags.SafelyGet("EntityParams");

            for (int i = 0; i < world.entitys.Count; i++)
            {
                world.entitys[i].Kill();
            }

            world.entitys.Clear();

            for (int i = 0; i < EntityCount; i++)
            {
                if (EntityX[i] == 5 && EntityY[i] == 5)
                {
                    EntityX[i] = 0;
                    EntityY[i] = 0;
                }

                Transform.SetObject(EntityTypes[i], new Position(EntityX[i], EntityY[i]), parameters: EntityParams[i]);
            }
        }
    }
}
