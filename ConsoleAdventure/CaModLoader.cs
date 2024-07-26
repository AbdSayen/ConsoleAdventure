using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CaModLoaderAPI;
using ConsoleAdventure.CaModLoaderAPI;

namespace ConsoleAdventure
{
    public static class CaModLoader
    {
        public static readonly string modsDirPath = Directory.GetCurrentDirectory() + @"\Content\mods";

        private static List<string> libs = new List<string> { "" };
        private static string[] modsPath;
        private static List<Mod> mods = new List<Mod>();

        public static List<Type> modItems = new List<Type>();
        public static List<GlobalItem> modGlobalItems = new List<GlobalItem>();
        public static List<Type> modTransforms = new List<Type>();

        public static Dictionary<Type, List<int>> modLoadedContentCount = new Dictionary<Type, List<int>>();


        public static void PreLoadMods()
        {
            modsPath = Directory.GetFiles(modsDirPath); // Получаем все файлы из папки модов
        }

        public static void LoadMods()
        {
            foreach (string path in modsPath)
            {
                // Загрузка сборки DLL
                Assembly assembly = Assembly.LoadFrom(path);

                string fileName = path.Replace("\\", "/").Split("/").Last().Split(".")[0];

                foreach (string lib in libs) // Пропускаем все библиотеки
                    if (fileName == lib) goto SkipLoop;

                Type type = assembly.GetType(fileName + "." + fileName); // Получение типа класса из загруженной сборки

                mods.Add((Mod)Activator.CreateInstance(type)); // Добавляем в список модов

                modLoadedContentCount.Add(type, new List<int> { 0, 0 }); // [0] - items, [1] - blocks

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

                foreach (Type block in exportedTypes.Where(type => type.IsSubclassOf(typeof(Transform)))) // Загружаем все блоки из модов
                {
                    Main.modTypesInitialized.Add(block, Main.modTypesInitialized.Keys.Count);
                    modTransforms.Add(block);
                    modLoadedContentCount[type][1]++;
                }

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

        public static List<Mod> GetActiveMods()
        {
            return mods;
        }
    }
}
