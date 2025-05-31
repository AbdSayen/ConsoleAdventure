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
using System.Text.Json;
using ConsoleAdventure.Content.Scripts;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Http;
using System.IO.Compression;
using System.Text;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.WorldEngine.Generate;
using ConsoleAdventure.Content.Scripts.WorldEngine;
using System.Security.Cryptography;

namespace ConsoleAdventure
{
    public static class CaModLoader
    {
        public static readonly string modsDirPath = Directory.GetCurrentDirectory() + @"\Content\mods";

        private static List<string> enabledMods = new List<string> { };
        private static List<string> disabledMods = new List<string> { };//, "GraphicsMod"  "NewMod"
        private static string[] modsPath;
        private static List<Mod> mods = new List<Mod>();
        internal static List<IMod> allMods = new List<IMod>();

        public static Dictionary<Type, int> modsIdsMap = new Dictionary<Type, int>();

        public static List<Type> modItems = new List<Type>();
        public static List<GlobalItem> modGlobalItems = new List<GlobalItem>();
        public static List<GlobalPlayer> modGlobalPlayers = new List<GlobalPlayer>();
        public static List<Type> modTransforms = new List<Type>();
        public static List<Buff> modBuffs = new List<Buff>();

        //public static Dictionary<Type, List<int>> modLoadedContentCount = new Dictionary<Type, List<int>>();  // [0] - items, [1] - blocks

        public static void UpdateModSourcesDLL()
        {
            string massageType = "[UpdateGameAPIForMods] ";

            ConsoleAdventure.logger.AddMessage(massageType + "Check if ModSources Folder exists");

            if (Directory.Exists(ModCreator.path))
            {
                ConsoleAdventure.logger.AddMessage(massageType + "ModSources Folder exists");
                string originalDLL = "ConsoleAdventure.dll";

                ConsoleAdventure.logger.AddMessage(massageType + "Check If consoleAdventure.dll in ModSources exists");
                if (!File.Exists(ModCreator.path + originalDLL))
                {
                    ConsoleAdventure.logger.AddMessage(massageType + "consoleAdventure.dll in ModSources not exists copy original dll");
                    File.Copy(originalDLL, ModCreator.path + originalDLL);
                }
                else
                {
                    ConsoleAdventure.logger.AddMessage(massageType + "consoleAdventure.dll in ModSources exists");
                    ConsoleAdventure.logger.AddMessage(massageType + "Compare checksums");
                    if (!GetChecksum(originalDLL).SequenceEqual(GetChecksum(ModCreator.path + originalDLL)))
                    {
                        ConsoleAdventure.logger.AddMessage(massageType + "Checksums not equal, replacing");
                        if (!File.Exists(originalDLL + "2"))
                            File.Copy(originalDLL, originalDLL+"2");
                        File.Replace(originalDLL+"2", ModCreator.path + originalDLL, originalDLL + ".backup");
                    }
                }

                string originalXML = "ConsoleAdventure.xml";

                ConsoleAdventure.logger.AddMessage(massageType + "Check If consoleAdventure.xml in ModSources exists");
                if (!File.Exists(ModCreator.path + originalXML))
                {
                    ConsoleAdventure.logger.AddMessage(massageType + "consoleAdventure.xml in ModSources not exists copy original xml documentation");
                    File.Copy(originalXML, ModCreator.path + originalXML);
                }
                else
                {
                    ConsoleAdventure.logger.AddMessage(massageType + "consoleAdventure.xml in ModSources exists");
                    ConsoleAdventure.logger.AddMessage(massageType + "Compare checksums");
                    if (!GetChecksum(originalXML).SequenceEqual(GetChecksum(ModCreator.path + originalXML)))
                    {
                        ConsoleAdventure.logger.AddMessage(massageType + "Checksums not equal, replacing");
                        if (!File.Exists(originalXML + "2"))
                            File.Copy(originalXML, originalXML + "2");
                        File.Replace(originalXML + "2", ModCreator.path + originalXML, originalXML + ".backup");
                    }
                }
            }
            ConsoleAdventure.logger.AddMessage(massageType + "Completed!");
        }

        static byte[] GetChecksum(string filePath)

        {

            using (BufferedStream stream = new BufferedStream(File.OpenRead(filePath), 1200000))
            {
                SHA256Managed sha = new SHA256Managed();

                byte[] checksum = sha.ComputeHash(stream);

                return checksum;

            }

        }

        public static async Task DownloadMod(string modName)
        {
            using (HttpClient client = new HttpClient())
            {
                ConsoleAdventure.progressBar.stepText = $"Downloading mod '{modName}'";
                ConsoleAdventure.progressBar.Progress = 0;
                ConsoleAdventure.menu.State = Content.Scripts.UI.MenuState.worldLoadingProgress;

                //string s = await client.GetStringAsync("https://consoleadventureofficial.github.io/bins/" + modName);

                long? responseLength = 0;
                StringBuilder sb = null;

                try
                {
                    using (HttpResponseMessage response = await client.GetAsync("https://consoleadventureofficial.github.io/bins/" + modName, HttpCompletionOption.ResponseHeadersRead))
                    {
                        responseLength = response.Content.Headers.ContentLength;
                        if (responseLength.HasValue)
                        {
                            using (Stream responseStream = await response.Content.ReadAsStreamAsync().ConfigureAwait(false))
                            using (StreamReader rdr = new StreamReader(responseStream))
                            {
                                sb = new StringBuilder(capacity: (int)responseLength.Value); // Note that `capacity` is in 16-bit UTF-16 chars, but responseLength is in bytes, though assuming UTF-8 it evens-out.

                                Char[] charBuffer = new Char[4096];
                                while (true)
                                {
                                    int read = await rdr.ReadAsync(charBuffer).ConfigureAwait(false);
                                    sb.Append(charBuffer, 0, read);

                                    if (read == 0)
                                    {
                                        break;
                                    }
                                    else
                                    {
                                        lock (sb)
                                        {
                                            //Program.game.Window.Title = String.Format(CultureInfo.CurrentCulture, "Read {0:N0} / {1:N0} chars (or bytes).", sb.Length, responseLength.Value);
                                            ConsoleAdventure.progressBar.Progress = (uint)(((float)sb.Length / responseLength.Value) * 100f);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                catch (HttpRequestException ex)
                {
                    ConsoleAdventure.menu.connectionLost = 180;
                    ConsoleAdventure.menu.CloseAllPages();
                    return;
                }

                byte[] data = Convert.FromBase64String(sb.ToString());

                using (MemoryStream memoryStream = new MemoryStream(data))
                {
                    using (ZipArchive zipArchive = new ZipArchive(memoryStream, ZipArchiveMode.Read))
                    {
                        string modDirPath = modsDirPath + "\\" + modName;
                        if (Directory.Exists(modDirPath))
                            Directory.Delete(modDirPath, true);
                        Directory.CreateDirectory(modDirPath);
                        zipArchive.ExtractToDirectory(modDirPath);
                    }
                }
            }

            ConsoleAdventure.menu.onlineMods = false;
            PreLoadMods();
            await ReloadMods();
            ConsoleAdventure.menu.ModsPanelInit();
            ConsoleAdventure.menu.State = Content.Scripts.UI.MenuState.mods;
        }

        public static async Task<List<IMod>> GetOnlineMods()
        {
            Dictionary<string, Dictionary<string, Dictionary<string, string>>> modb = new Dictionary<string, Dictionary<string, Dictionary<string, string>>>();

            using (HttpClient client = new HttpClient())
            {
                string s = "";
                try
                {
                    s = await client.GetStringAsync("https://consoleadventureofficial.github.io/mods");
                }
                catch (HttpRequestException ex)
                {
                    return null;
                }
                modb = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, Dictionary<string, string>>>>(s);
            }

            List<IMod> onlineMods = new List<IMod>();

            for (int i = 0; i < modb.Count; i++)
            {
                string modDir = modb.Keys.ToArray()[i];

                var modSettings = modb[modDir]["Settings"];

                EmptyMod modData = new()
                {
                    dirName = modDir,
                    modName = modSettings["Name"],
                    modAuthor = modSettings["Author"],
                    modVersion = modSettings["Version"],
                    modDescription = modSettings["Description"],
                    modIcon = CharTexture.Read(modSettings["Icon"])
                };

                onlineMods.Add(modData);
            }

            return onlineMods;
        }

        public static void EnableMod(string mod)
        {
            enabledMods.Add(mod);
        }

        public static void DisableMod(string mod)
        {
            enabledMods.Remove(mod);
        }

        public static List<string> ReadSavedEnabledMods()
        {
            string enabledModsFile = modsDirPath + @"\enabled-mods.json";

            if (!File.Exists(enabledModsFile))
            {
                string data = JsonSerializer.Serialize(new List<string>());
                File.WriteAllText(enabledModsFile, data);
            }

            string json = File.ReadAllText(enabledModsFile);

            return JsonSerializer.Deserialize<List<string>>(json);
        }

        public static void SaveEnabledMods()
        {
            string enabledModsFile = modsDirPath + @"\enabled-mods.json";

            if (File.Exists(enabledModsFile))
            {
                string data = JsonSerializer.Serialize(enabledMods);
                File.WriteAllText(enabledModsFile, data);
            }
        }

        public static void PreLoadMods()
        {
            enabledMods = ReadSavedEnabledMods();

            if (!Directory.Exists(modsDirPath))
            {
                Directory.CreateDirectory(modsDirPath);
            }

            modsPath = Directory.GetDirectories(modsDirPath); // Получаем все папки из папки модов
        }

        public static Task ReloadMods()
        {
            ConsoleAdventure.progressBar.stepText = Localization.GetTranslation("Progress", "LoadMods");
            ConsoleAdventure.progressBar.Progress = 0;
            ConsoleAdventure.menu.State = Content.Scripts.UI.MenuState.worldLoadingProgress;
            ConsoleAdventure.menu.worldErrorList = null;

            Thread load = new Thread(new ThreadStart(Load));
            load.Start();

            void Load()
            {
                try
                {
                    LoadMods(true);
                }

                catch (Exception ex)
                {
                    string error = $"{ex.GetType()}: {ex.Message}\n{ex.InnerException}\n{ex.StackTrace}\n{ex.Source}\n{ex.TargetSite}";
                    ConsoleAdventure.menu.OpenWorldError(error);
                    ConsoleAdventure.logger.AddException(error);
                }
            }

            return Task.Run(() => load.Join());
        }

        public static void LoadMods(bool reload = false)
        {
            if (reload)
            {
                UnloadMods();
                disabledMods = new();
                modsIdsMap = new();
                modItems = new();
                modGlobalItems = new();
                modGlobalPlayers = new();
                modTransforms = new();
                modBuffs = new();
                mods = new();
                allMods = new();
                Main.modTypesInitialized = new();
                Main.AllTransformCount = Main.vanillaTypesInitialized;
                Main.modTransformTypes.Clear();
                Localization.Load();
            }

            for (int i = 0; i < modsPath.Length; i++)
            {
                string dirName = modsPath[i].Replace("\\", "/").Split("/").Last();
                if (!enabledMods.Contains(dirName))
                {
                    disabledMods.Add(dirName);
                }
            }

            int transformsFromAllMods = 0;

            for (int i = 0; i < modsPath.Length; i++)
            {
                string path = modsPath[i];

                if (reload)
                {
                    ConsoleAdventure.progressBar.Progress = (uint)((float)(i + 1) / modsPath.Length * 100);
                }

                string[] modFiles = Directory.GetFiles(path); // Получаем все файлы из папки модов
                string dirName = Path.GetFileName(path);

                string buildFile = path + "\\build.json";

                if (File.Exists(buildFile))
                {
                    foreach (string file in modFiles)
                    {
                        string fileName = file.Replace("\\", "/").Split("/").Last().Split(".")[0];

                        if (Path.GetExtension(file) == ".dll")
                        {
                            if (fileName != dirName) goto SkipLoop;

                            string buildText = File.ReadAllText(buildFile); // Читаем жисон
                            Dictionary<string, Dictionary<string, string>> build = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(buildText); // Делаем из жисона наш словарь

                            var modSettings = build["Settings"];

                            EmptyMod modData = new() // балванка мода, чтобы показовались и выключенные моды
                            {
                                dirName = dirName,
                                modName = modSettings["Name"],
                                modAuthor = modSettings["Author"],
                                modVersion = modSettings["Version"],
                                modDescription = modSettings["Description"],
                                modIcon = CharTexture.Read(modSettings["Icon"])
                            };

                            foreach (string modName in disabledMods) // Пропускаем выключенные моды
                                if (fileName == modName) { allMods.Add(modData); goto SkipLoop; }

                            // Загрузка сборки DLL
                            Assembly assembly = Assembly.LoadFrom(file);

                            Type type = assembly.GetType(fileName + "." + fileName); // Получение типа класса из загруженной сборки
                            Mod mod = (Mod)Activator.CreateInstance(type);
                            mod.dirName = dirName;
                            mod.modName = modData.modName;
                            mod.modAuthor = modData.modAuthor;
                            mod.modVersion = modData.modVersion;
                            mod.modDescription = modData.modDescription;
                            mod.modIcon = modData.modIcon;

                            mods.Add(mod); // Добавляем в список модов
                            allMods.Add(mod); // Добавляем в список всех модов
                            modsIdsMap.Add(type, mods.Count - 1);

                            Localization.LoadModTranslations(path + "\\Content\\Localization");

                            Main.modTypesInitialized.Add(fileName, new Dictionary<Type, int>());

                            //modLoadedContentCount.Add(type, new List<int> { 0, 0 }); // [0] - items [1] - transforms

                            Type[] exportedTypes = assembly.GetExportedTypes();
                            Type entityType = typeof(Entity);

                            foreach (Type item in exportedTypes.Where(type => type.IsSubclassOf(typeof(ModItem)) || type.IsSubclassOf(typeof(Item)))) // Загружаем все предметы из модов
                            {
                                modItems.Add(item);

                                if(!item.IsAbstract)
                                    mod.ItemsCount++;
                                //modLoadedContentCount[type][0]++;
                            }

                            foreach (Type gItem in exportedTypes.Where(type => type.IsSubclassOf(typeof(GlobalItem)))) // Загружаем все глобальные предметы из модов
                            {
                                GlobalItem gi = (GlobalItem)Activator.CreateInstance(gItem);
                                modGlobalItems.Add(gi);
                            }

                            foreach (Type player in exportedTypes.Where(type => type.IsSubclassOf(typeof(GlobalPlayer)))) // Загружаем все глобальных Игроков из модов
                            {
                                GlobalPlayer gp = (GlobalPlayer)Activator.CreateInstance(player);
                                modGlobalPlayers.Add(gp);
                            }

                            foreach (Type transform in exportedTypes.Where(type => type.IsSubclassOf(typeof(Transform)))) // Загружаем все трансформы из модов
                            {
                                Main.modTypesInitialized[fileName].Add(transform, Main.modTypesInitialized[fileName].Keys.Count);
                                modTransforms.Add(transform);

                                if (!transform.IsAbstract)
                                {
                                    mod.TransformsCount++;

                                    if (transform.IsSubclassOf(entityType)) 
                                    {
                                        mod.EntitiesCount++;
                                    }
                                }
                            }

                            foreach (Type buff in exportedTypes.Where(type => type.IsSubclassOf(typeof(Buff)))) // Загружаем все бафы из модов
                            {
                                Buff aBuff = (Buff)Activator.CreateInstance(buff);
                                modBuffs.Add(aBuff);

                                if(!buff.IsAbstract)
                                    mod.BuffsCount++;
                            }

                            //Main.modTransformTypesOffset.Add(fileName, (byte)transformsFromAllMods);

                            transformsFromAllMods += mod.TransformsCount; //modLoadedContentCount[type][1];

                        SkipLoop: continue; // Метка для пропуска внешнего цикла
                        }
                    }
                }
            }

            if (reload)
            {
                ConsoleAdventure.menu.State = Content.Scripts.UI.MenuState.mainScreen;

                InitializeMods();
                Main.InitTransformsTypes(Main.vanillaTypesInitialized);
                WorldIO.InitContent();
                ConsoleAdventure.menu.MenuInit();
                RunMods();
            }
        }

        public static List<Mod> GetActiveMods()
        {
            return mods;
        }

        public static void InitializeMods()
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].Init();
            }
        }

        public static void RunMods()
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].Run();
            }
        }

        public static void WorldLoadedMods(World world)
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].WorldLoaded(world);
            }
        }

        public static void WorldGeneratorBuildPipelineMods(Generator generator)
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].WorldGeneratorBuildPipeline(generator);
            }
        }

        public static bool WorldGeneratorPreBuildPipelineMods(Generator generator)
        {
            bool generate = true;
            for (int i = 0; i < mods.Count; i++)
            {
                bool d = mods[i].WorldGeneratorPreBuildPipeline(generator);
                if (generate)
                    generate = d;
            }

            return generate;
        }

        public static void WorldPropertiesBuildPipelineMods(Generator generator)
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].WorldPropertiesBuildPipeline(generator);
            }
        }

        public static bool WorldPropertiesPreBuildPipelineMods(Generator generator)
        {
            bool generate = true;
            for (int i = 0; i < mods.Count; i++)
            {
                bool d = mods[i].WorldPropertiesPreBuildPipeline(generator);
                if (generate)
                    generate = d;
            }

            return generate;
        }

        public static void WorldPostGenerateMods(World world)
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].WorldPostGenerate(world);
            }
        }

        public static Observer GetWorldObserverMods(World world)
        {
            Observer ob = null;
            for (int i = 0; i < mods.Count; i++)
            {
                Observer o = mods[i].GetWorldObserver(world);
                if (ob == null)
                    ob = o;
            }
            return ob;
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

        public static void PostWorldUpdateMods(World world)
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].PostWorldUpdate(world);
            }
        }

        public static void PreWorldUpdateMods(World world)
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].PreWorldUpdate(world);
            }
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

        public static void UnloadMods()
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].Unload();
            }
        }

        public static void SaveModTagsMods(Tags tags)
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].SaveModTags(new ModTags(tags, mods[i].dirName));
            }
        }

        public static void LoadModTagsMods(Tags tags)
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].LoadModTags(new ModTags(tags, mods[i].dirName));
            }
        }

        public static void PreSaveWorldMods()
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].PreSaveWorld();
            }
        }

        public static void PreLoadWorldMods()
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].PreLoadWorld();
            }
        }

        public static void PostInitContentMods()
        {
            for (int i = 0; i < mods.Count; i++)
            {
                mods[i].PostInitContent();
            }
        }
    }
}