using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CaModLoaderAPI;
using ConsoleAdventure.CaModLoaderAPI;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure
{
    public static class CaModLoader
    {
        public static readonly string modsDirPath = Directory.GetCurrentDirectory() + @"\Content\mods";

        private static List<string> libs = new List<string> { "" };
        private static string[] modsPath;
        private static List<Mod> mods = new List<Mod>();

        public static Dictionary<Type, int> modsIdsMap = new Dictionary<Type, int>();

        public static List<Type> modItems = new List<Type>();
        public static List<GlobalItem> modGlobalItems = new List<GlobalItem>();
        public static List<Type> modTransforms = new List<Type>();

        public static Dictionary<Type, List<int>> modLoadedContentCount = new Dictionary<Type, List<int>>();  // [0] - items, [1] - blocks


        public static void PreLoadMods()
        {
            modsPath = Directory.GetFiles(modsDirPath); // Получаем все файлы из папки модов
        }

        public static void LoadMods()
        {
            int transformsFromAllMods = 0;

            foreach (string path in modsPath)
            {
                // Загрузка сборки DLL
                Assembly assembly = Assembly.LoadFrom(path);

                string fileName = path.Replace("\\", "/").Split("/").Last().Split(".")[0];

                foreach (string lib in libs) // Пропускаем все библиотеки
                    if (fileName == lib) goto SkipLoop;

                Type type = assembly.GetType(fileName + "." + fileName); // Получение типа класса из загруженной сборки
                Mod mod = (Mod)Activator.CreateInstance(type);
                mods.Add(mod); // Добавляем в список модов
                modsIdsMap.Add(type, mods.Count - 1);

                Main.modTypesInitialized.Add(fileName, new Dictionary<Type, int>());

                modLoadedContentCount.Add(type, new List<int> { 0, 0 }); // [0] - items [1] - transforms

                Type[] exportedTypes = assembly.GetExportedTypes();

                foreach (Type item in exportedTypes.Where(type => type.IsSubclassOf(typeof(ModItem)))) // Загружаем все предметы из модов
                {
                    modItems.Add(item);
                    modLoadedContentCount[type][0]++;
                }

                foreach (Type item in exportedTypes.Where(type => type.IsSubclassOf(typeof(GlobalItem)))) // Загружаем все глобальные предметы из модов
                {
                    GlobalItem gi = (GlobalItem)Activator.CreateInstance(item);
                    modGlobalItems.Add(gi);
                }

                foreach (Type block in exportedTypes.Where(type => type.IsSubclassOf(typeof(Transform)))) // Загружаем все трансформы из модов
                {
                    Main.modTypesInitialized[fileName].Add(block, Main.modTypesInitialized[fileName].Keys.Count);
                    modTransforms.Add(block);
                    modLoadedContentCount[type][1]++;
                }

                Main.modTransformTypesOffset.Add(fileName, (byte)transformsFromAllMods);

                transformsFromAllMods += modLoadedContentCount[type][1];

            SkipLoop: continue; // Метка для пропуска внешнего цикла
            }
        }

        public static void InitializeMods()
        {
            foreach (Mod mod in mods)
            {
                mod.Init();
            }
        }

        public static void RunMods()
        {
            foreach (Mod mod in mods)
            {
                mod.Run();
            }
        }

        public static void WorldLoadedMods(World world)
        {
            foreach (Mod mod in mods)
            {
                mod.WorldLoaded(world);
            }
        }

        public static void PreDrawMods(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].PreDraw(spriteBatch, gameTime);
            }
            spriteBatch.End();
        }

        public static void PostDrawMods(SpriteBatch spriteBatch, GameTime gameTime)
        {
            spriteBatch.Begin();
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].PostDraw(spriteBatch, gameTime);
            }
            spriteBatch.End();
        }

        public static bool PreDrawWorldMods(SpriteBatch spriteBatch, GameTime gameTime, World world)
        {
            bool drawWorld = true;
            for (int i = 0; i < mods.Count; i++)
            {
                bool d = mods[i].PreDrawWorld(spriteBatch, gameTime, world);
                if (drawWorld)
                    drawWorld = d;
            }

            return drawWorld;
        }

        public static void PostDrawWorldMods(SpriteBatch spriteBatch, GameTime gameTime, World world)
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].PostDrawWorld(spriteBatch, gameTime, world);
            }
        }


        public static List<Mod> GetActiveMods()
        {
            return mods;
        }
    }
}
