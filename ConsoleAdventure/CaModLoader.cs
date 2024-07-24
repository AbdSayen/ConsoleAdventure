using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using CaModLoaderAPI;

namespace ConsoleAdventure
{
    public static class CaModLoader
    {
        public static readonly string modsDirPath = Directory.GetCurrentDirectory() + @"\Content\mods";

        private static List<string> libs = new List<string> { "" };
        private static string[] modsPath;
        private static List<Mod> mods = new List<Mod>();
        

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
