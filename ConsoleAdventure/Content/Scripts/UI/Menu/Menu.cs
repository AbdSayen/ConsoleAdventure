using CaModLoaderAPI;
using ConsoleAdventure.Content.Scripts.Audio;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Graphics.PackedVector;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleAdventure.Content.Scripts.UI
{
    public class Menu
    {
        public int State; // { get; private set; }

        internal MenuButton[] menuButtons = new MenuButton[4];

        internal MenuButton[] menuSettingsButtons = new MenuButton[3];

        private List<WorldPanel> worldPanels = new List<WorldPanel>();

        private InfoPanel aboutGamePanel = null;

        private InfoPanel aboutControlPanel = null;

        private InfoPanel modListPanel = null;

        private static int worldDrawBuffer = 6;

        private int startWList, endWList = worldDrawBuffer;

        private int selectedLanguage = SettingsSystem.GetSetting("Options", "Language"); // Получить сохраненный язык

        private string modsListText = "";

        public Menu()
        {
            MenuInit();
            WorldMenuInit();
        }

        public void WorldMenuInit()
        {
            var worlds = WorldIO.GetWorlds();


            for (int i = 0; i < worlds.names.Length; i++)
            {
                worldPanels.Add(new WorldPanel(new Rectangle(), worlds.names[i], worlds.seeds[i].ToString()));
            }

            if (worldPanels.Count > 0)
                worldPanels[0].isHover = true;
        }

        public void MenuInit()
        {
            byte[] menuButtonTypes = new byte[4] { 0, 1, 3, 2 };

            byte[] menuSettingsButtonTypes = new byte[3] { 0, 1, 2 };

            modsListText = String.Empty;
            foreach (Mod mod in CaModLoader.GetActiveMods())
            {
                string newDescription = String.Empty;
                int symbolsThreshold = 50;
                int currentThreshold = symbolsThreshold;
                int oldLineLength = 0;
                foreach (string word in mod.modDescription.Replace("\n", " _ ").Split(" "))
                {
                    oldLineLength = newDescription.Split("\n").Last().Length;
                    if (word == "_")
                    {
                        newDescription += "\n    ";
                        currentThreshold += oldLineLength - 1;
                        continue;
                    }
                    else if ((newDescription + word).Length > currentThreshold)
                    {
                        newDescription += "\n    ";
                        currentThreshold += symbolsThreshold;
                    }
                    newDescription += word + " ";
                }

                string itemsInMod = CaModLoader.modLoadedContentCount[mod.GetType()][0].ToString();
                string blocksInMod = CaModLoader.modLoadedContentCount[mod.GetType()][1].ToString();

                string textItems = CaModLoader.modLoadedContentCount[mod.GetType()][0] != 1 ? TextAssets.ItemsGenitive : TextAssets.Item;
                string textBlocks = CaModLoader.modLoadedContentCount[mod.GetType()][1] != 1 ? TextAssets.BlocksGenitive : TextAssets.Block; ;

                modsListText += " - " + mod.modName + " v" + mod.modVersion + " by " + mod.modAuthor + "   | " + itemsInMod + " " + textItems + " " + blocksInMod + " " + textBlocks + "\n    " + newDescription + "\n\n";
            }

            aboutGamePanel = new InfoPanel(new Rectangle((ConsoleAdventure.screenWidth / 2) - 28 * 9, (ConsoleAdventure.screenHeight / 2) - 20 * 18, 64, 30), TextAssets.About, TextAssets.AboutGame);

            aboutControlPanel = new InfoPanel(new Rectangle((ConsoleAdventure.screenWidth / 2) - 28 * 9, (ConsoleAdventure.screenHeight / 2) - 20 * 18, 64, 30), TextAssets.Control, TextAssets.AboutControl);

            modListPanel = new InfoPanel(new Rectangle((ConsoleAdventure.screenWidth / 2) - 28 * 9, (ConsoleAdventure.screenHeight / 2) - 20 * 18, 64, 30), TextAssets.Mods, modsListText);

            for (int i = 0; i < menuSettingsButtons.Length; i++)
            {
                string text = "";

                switch (menuSettingsButtonTypes[i])
                {
                    case 0:
                        text = "Language";
                        break;
                    case 1:
                        text = "About";
                        break;
                    case 2:
                        text = "Control";
                        break;
                }
                int startPos = 320;
                int indent = 25;

                Vector2 SettingButtonPos = new Vector2((ConsoleAdventure.Width / 2), startPos + (indent * i));

                menuSettingsButtons[i] = new MenuButton(SettingButtonPos, text, new Color(230, 230, 230), menuSettingsButtonTypes[i]);
                
                if(menuSettingsButtonTypes[i] == 0)
                {
                    menuSettingsButtons[i].text += Localization.GetLanguageName(selectedLanguage);
                    menuSettingsButtons[i].Position = SettingButtonPos;
                    menuSettingsButtons[i].Center = SettingButtonPos;
                }
            }
            int curentMenuBatton = -1;

            for (int i = 0; i < menuButtons.Length; i++)
            {
                string text = "";

                switch (menuButtonTypes[i])
                {
                    case 0:
                        text = "Play";
                        break;
                    case 1:
                        text = "Settings";
                        break;
                    case 2:
                        text = "Exit";
                        break;
                    case 3:
                        text = "Mods";
                        break;
                }
                int startPos = 220;
                int indent = 100;

                if (menuButtons[i] != null && menuButtons[i].isHover)
                    curentMenuBatton = i;

                menuButtons[i] = new MenuButton(new Vector2((ConsoleAdventure.Width / 2) + (indent * (i - 1.5f)), startPos), text, new Color(230, 230, 230), menuButtonTypes[i]);

                if (curentMenuBatton == i)
                {
                    if(State > 0)
                        menuButtons[i].cursorColor = Color.Red;
                    menuButtons[i].isHover = true;
                }
            }

            if (curentMenuBatton == -1)
                menuButtons[0].isHover = true;

            menuSettingsButtons[0].isHover = true;

            ConsoleAdventure.progressBar = new ProgressBar(new Rectangle(new Point((int)ConsoleAdventure.Width / 2, ((int)ConsoleAdventure.Height / 2) - 120), new Point(50 * 9, 19)), Color.LightGreen, 50, ProgressBar.PercentRight);
        }

        int timer;
        public void MenuUpdate()
        {
            for (int i = 0; i < menuButtons.Length; i++)
            {
                int waitTime = Utils.StabilizeTicks(20);

                if (State == 0)
                {
                    if (ConsoleAdventure.kstate.IsKeyDown(Keys.Right) && timer >= waitTime) //прокрутка 
                    {
                        if (menuButtons[i].isHover)
                        {
                            menuButtons[i].isHover = false;

                            if (i != menuButtons.Length - 1) //перемещяем курсор
                                menuButtons[i + 1].isHover = true;
                            else
                                menuButtons[0].isHover = true;

                            timer = 0;
                        }
                    }

                    if (ConsoleAdventure.kstate.IsKeyDown(Keys.Left) && timer >= waitTime)
                    {
                        if (menuButtons[i].isHover)
                        {
                            menuButtons[i].isHover = false;

                            if (i != 0)
                                menuButtons[i - 1].isHover = true;
                            else
                                menuButtons[menuButtons.Length - 1].isHover = true;

                            timer = 0;
                        }    
                    }

                    if (menuButtons[i].isHover && ConsoleAdventure.kstate.IsKeyDown(Keys.Enter))
                    {
                        if (menuButtons[i].type == 0)
                        {
                            State = 1; 
                            ConsoleAdventure.display = new Display(ConsoleAdventure.world);
                            menuButtons[i].cursorColor = Color.Red;
                            timer = 0;
                        }
                        if (menuButtons[i].type == 1)
                        {
                            menuButtons[i].cursorColor = Color.Red;
                            State = 2;
                            timer = 0;
                        }
                        if (menuButtons[i].type == 3)
                        {
                            State = 5;
                            timer = 0;
                        }
                        if (menuButtons[i].type == 2)
                        {
                            ConsoleAdventure.isExit = true;
                        }
                    }
                }
            }

            if (State == 1)
            {
                if (worldPanels.Count > 0)
                {
                    for (int i = 0; i < worldPanels.Count; i++)
                    {
                        int waitTime = Utils.StabilizeTicks(10);

                        if (ConsoleAdventure.kstate.IsKeyDown(Keys.Up) && timer >= waitTime && (i - 1) != -1) //прокрутка
                        {
                            if (worldPanels[i].isHover)
                            {
                                worldPanels[i].isHover = false;
                                int newIndex = (i - 1 + worldPanels.Count) % worldPanels.Count; //находим следующий мир
                                worldPanels[newIndex].isHover = true;

                                if (newIndex < startWList) //прокручиваем область видимого списка
                                {
                                    startWList = newIndex;
                                    endWList = Math.Min(startWList + worldDrawBuffer, worldPanels.Count);
                                }

                                timer = 0;
                                break;
                            }
                        }

                        if (ConsoleAdventure.kstate.IsKeyDown(Keys.Down) && timer >= waitTime && i < worldPanels.Count - 1)
                        {
                            if (worldPanels[i].isHover)
                            {
                                worldPanels[i].isHover = false;
                                int newIndex = (i + 1) % worldPanels.Count;
                                worldPanels[newIndex].isHover = true;

                                if (newIndex >= endWList)
                                {
                                    startWList = (startWList + 1) % worldPanels.Count;
                                    endWList = Math.Min(startWList + worldDrawBuffer, worldPanels.Count);
                                }

                                timer = 0;
                                break;
                            }
                        }

                        if (ConsoleAdventure.kstate.IsKeyDown(Keys.Enter) && timer >= Utils.StabilizeTicks(30))
                        {
                            if (worldPanels[i].curssor == 0 && worldPanels[i].isHover)
                            {
                                ConsoleAdventure.progressBar.stepText = Localization.GetTranslation("Progress", "LoadFile");
                                ConsoleAdventure.progressBar.Progress = 0;

                                string name = worldPanels[i].name;
                                State = 6;

                                Thread load = new Thread(new ThreadStart(LoadWorld));
                                load.Start();

                                timer = 0;

                                void LoadWorld()
                                {
                                    ConsoleAdventure.CreateWorld(name, 1234, false);
                                    WorldIO.Load(name);
                                }
                            }

                            if (worldPanels[i].curssor == 2 && worldPanels[i].isHover)
                            {
                                WorldIO.Delete(worldPanels[i].name);
                                worldPanels.Clear();
                                WorldMenuInit();
                                timer = 0;
                            }
                        }
                    }

                    if (ConsoleAdventure.kstate.IsKeyDown(Keys.Left) && timer >= Utils.StabilizeTicks(15)) //прокрутка кнопок мира
                    {
                        for (int i = 0; i < worldPanels.Count; i++)
                        {
                            worldPanels[i].curssor = (worldPanels[i].curssor - 1 + 3) % 3;
                        }
                        timer = 0;
                    }

                    if (ConsoleAdventure.kstate.IsKeyDown(Keys.Right) && timer >= Utils.StabilizeTicks(15))
                    {
                        for (int i = 0; i < worldPanels.Count; i++)
                        {
                            worldPanels[i].curssor = (worldPanels[i].curssor + 1) % 3;
                        }
                        timer = 0;
                    }
                }

                if (!ConsoleAdventure.kstate.IsKeyDown(Keys.N) && ConsoleAdventure.prekstate.IsKeyDown(Keys.N))
                {
                    ConsoleAdventure.CreateWorld("World" + (worldPanels.Count > 0 ? worldPanels.Count : ""), ConsoleAdventure.rand.Next(0, 100000000));
                    WorldIO.Save(ConsoleAdventure.world.name);
                    worldPanels.Clear();
                    WorldMenuInit();
                }

                if (!ConsoleAdventure.kstate.IsKeyDown(Keys.F) && ConsoleAdventure.prekstate.IsKeyDown(Keys.F))
                {
                    timer = 0;
                    State = 0;
                    for (int i = 0; i < menuButtons.Length; i++)
                    {
                        menuButtons[i].cursorColor = Color.Yellow;
                    }
                    Melody();
                }
            }

            if (State == 2)
            {
                for (int i = 0; i < menuSettingsButtons.Length; i++)
                {
                    int waitTime = Utils.StabilizeTicks(10);

                    if (menuSettingsButtons[i].isHover && i == 0)
                    {
                        
                        if (ConsoleAdventure.kstate.IsKeyDown(Keys.Right) && timer >= waitTime)
                        {
                            if (selectedLanguage < 1)
                                selectedLanguage += 1;

                            ReLocalize();
                            menuSettingsButtons[i].text += Localization.GetLanguageName(selectedLanguage);

                            timer = 0;
                        }
                        if (ConsoleAdventure.kstate.IsKeyDown(Keys.Left) && timer >= waitTime)
                        {
                            if (selectedLanguage > 0)
                                selectedLanguage -= 1;

                            ReLocalize();
                            menuSettingsButtons[i].text += Localization.GetLanguageName(selectedLanguage);

                            timer = 0;
                        }
                        menuSettingsButtons[i].text = Localization.GetTranslation("UI", "Language") + Localization.GetLanguageName(selectedLanguage);
                        menuSettingsButtons[i].UpdateRectToTextSize();
                    }

                    if (ConsoleAdventure.kstate.IsKeyDown(Keys.Up) && timer >= waitTime)
                    {
                        if (menuSettingsButtons[i].isHover)
                        {
                            menuSettingsButtons[i].isHover = false;

                            if (i != 0)
                                menuSettingsButtons[i - 1].isHover = true;
                            else
                                menuSettingsButtons[menuSettingsButtons.Length - 1].isHover = true;

                            timer = 0;
                        }
                    }

                    if (ConsoleAdventure.kstate.IsKeyDown(Keys.Down) && timer >= waitTime)
                    {
                        if (menuSettingsButtons[i].isHover)
                        {
                            menuSettingsButtons[i].isHover = false;

                            if (i < menuSettingsButtons.Length - 1)
                                menuSettingsButtons[i + 1].isHover = true;
                            else
                                menuSettingsButtons[0].isHover = true;

                            timer = 0;
                        }
                    }

                    if (menuSettingsButtons[i].isHover && ConsoleAdventure.kstate.IsKeyDown(Keys.Enter) && timer >= Utils.StabilizeTicks(30))
                    {
                        if (menuSettingsButtons[i].type == 1)
                        {
                            State = 3;
                        }

                        if (menuSettingsButtons[i].type == 2)
                        {
                            State = 4;
                        }
                        timer = 0;
                    }
                }

                if (ConsoleAdventure.kstate.IsKeyDown(Keys.Left) && timer >= Utils.StabilizeTicks(15)) //прокрутка кнопок мира
                {
                    for (int i = 0; i < worldPanels.Count; i++)
                    {
                        worldPanels[i].curssor = (worldPanels[i].curssor - 1 + 3) % 3;
                    }
                    timer = 0;
                }

                if (ConsoleAdventure.kstate.IsKeyDown(Keys.Right) && timer >= Utils.StabilizeTicks(15))
                {
                    for (int i = 0; i < worldPanels.Count; i++)
                    {
                        worldPanels[i].curssor = (worldPanels[i].curssor + 1) % 3;
                    }
                    timer = 0;
                }
            }

            if (State == 5)
            {
                if (ConsoleAdventure.kstate.IsKeyDown(Keys.Enter) && timer >= Utils.StabilizeTicks(30))
                {
                    Utils.OpenExplorerAtFolder(CaModLoader.modsDirPath);
                    timer = 0;
                }
            }

            if (ConsoleAdventure.kstate.IsKeyDown(Keys.Escape) && timer >= Utils.StabilizeTicks(20))
            {
                CloseAllPages();
                timer = 0;
            }

            if (State == 0 || State == 2)
            {
                if (ConsoleAdventure.kstate.IsKeyDown(Keys.Left) && !ConsoleAdventure.prekstate.IsKeyDown(Keys.Left) ||
                    ConsoleAdventure.kstate.IsKeyDown(Keys.Right) && !ConsoleAdventure.prekstate.IsKeyDown(Keys.Right) ||
                    ConsoleAdventure.kstate.IsKeyDown(Keys.Up) && !ConsoleAdventure.prekstate.IsKeyDown(Keys.Up) ||
                    ConsoleAdventure.kstate.IsKeyDown(Keys.Down) && !ConsoleAdventure.prekstate.IsKeyDown(Keys.Down))
                {
                    TickSound();
                }
            }
            timer++;
        }
        
        public void CloseAllPages()
        {
            State = 0;
            for (int i = 0; i < menuButtons.Length; i++)
            {
                menuButtons[i].cursorColor = Color.Yellow;
            }
        }

        private void ReLocalize()
        {
            SettingsSystem.SetSetting("Options", "Language", selectedLanguage); // Сохранить выбранный язык
            TextAssets.UpdateLabels();
            MenuInit();
            menuButtons[1].isHover = true;
            menuButtons[1].cursorColor = Color.Red;
            menuButtons[0].isHover = false;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Begin(samplerState: SamplerState.PointClamp);
            spriteBatch.DrawString(ConsoleAdventure.Font, TextAssets.logo, new Vector2((ConsoleAdventure.Width / 2) - (621 / 2), 20), Color.White);
            spriteBatch.DrawString(ConsoleAdventure.Font, Docs.GetInfo(), new Vector2(10, ConsoleAdventure.Height - 25), Color.White);
            spriteBatch.DrawString(ConsoleAdventure.Font, TextAssets.navigHelp, new Vector2(ConsoleAdventure.Width - (ConsoleAdventure.Font.MeasureString(TextAssets.navigHelp).X + 10), ConsoleAdventure.Height - 25), Color.Gray);
            
            if (State > 0)
            {
                spriteBatch.DrawString(ConsoleAdventure.Font, TextAssets.navigHelpBack, new Vector2(ConsoleAdventure.Width - (ConsoleAdventure.Font.MeasureString(TextAssets.navigHelpBack).X + 10), ConsoleAdventure.Height - 50), Color.Gray);
            }

            for (int i = 0; i < menuButtons.Length; i++)
            {
                menuButtons[i].Draw(spriteBatch);
            }

            if (State == 1)
            {
                if (worldPanels.Count > 0)
                {
                    int number = 0;
                    for (int i = startWList; i < endWList; i++)
                    {
                        if (i < worldPanels.Count) //рисуем видемую оласть списка миров
                        {
                            worldPanels[i].Center = new Vector2((ConsoleAdventure.Width / 2) - 207, (number * (19 * 4)) + 9 * 30);
                            worldPanels[i].Draw(spriteBatch);
                            number++;
                        }
                    }

                    spriteBatch.DrawString(ConsoleAdventure.Font, TextAssets.navigHelpWorld, new Vector2(ConsoleAdventure.Width - (ConsoleAdventure.Font.MeasureString(TextAssets.navigHelpBack).X + 10) + 9 * 12, ConsoleAdventure.Height - 114), Color.Gray);
                }

                else
                {
                    spriteBatch.DrawString(ConsoleAdventure.Font, TextAssets.HelpWorldCreate, new Vector2(ConsoleAdventure.Width / 2 - (ConsoleAdventure.Font.MeasureString(TextAssets.HelpWorldCreate).X / 2), ConsoleAdventure.Height / 2 - 180), Color.Gray);
                }
            }

            if (State == 2 || State == 3 || State == 4)
            {
                for (int i = 0; i < menuSettingsButtons.Length; i++)
                {
                    menuSettingsButtons[i].Draw(spriteBatch);
                }
            }

            if (State == 3)
            {
                aboutGamePanel.Draw(spriteBatch);
            }

            if (State == 4)
            {
                aboutControlPanel.Draw(spriteBatch);
            }

            if (State == 5)
            {
                modListPanel.Draw(spriteBatch);
                spriteBatch.DrawString(ConsoleAdventure.Font, TextAssets.navigModFolderHelp, new Vector2(ConsoleAdventure.Width/2 - ConsoleAdventure.Font.MeasureString(TextAssets.navigHelp).X, ConsoleAdventure.Height - 25), Color.Gray);
            }

            if (State == 6)
            {
                ConsoleAdventure.progressBar.Draw(spriteBatch);
            }

            spriteBatch.End();
        }

        public async static void TickSound()
        {
            await SoundEngine.PlaySound(SoundEngine.BubbleWave, 500, 0.1f, TimeSpan.FromMilliseconds(500));   
        }

        public async static void Melody()
        {
            await SoundEngine.PlaySound(SoundEngine.FadeInWhiteNoiseWave, 500, 0.25f, TimeSpan.FromMilliseconds(500));
            SoundEngine.PlaySound(SoundEngine.FadeOutWhiteNoiseWave, 500, 0.4f, TimeSpan.FromMilliseconds(500));
            SoundEngine.PlaySound(SoundEngine.SawtootWave, 146, 0.10f, TimeSpan.FromMilliseconds(2250));
            SoundEngine.PlaySound(SoundEngine.SawtootWave, 73, 0.10f, TimeSpan.FromMilliseconds(2250));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 293, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 293, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 293, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 293, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 587, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 587, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 440, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 440, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.FadeOutWhiteNoiseWave, 500, 0.35f, TimeSpan.FromMilliseconds(500));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 440, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 440, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 415, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 415, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 392, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 392, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.FadeOutWhiteNoiseWave, 500, 0.35f, TimeSpan.FromMilliseconds(500));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 349, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 349, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 293, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 293, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 349, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 349, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 392, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 392, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.FadeOutWhiteNoiseWave, 500, 0.35f, TimeSpan.FromMilliseconds(500));
            SoundEngine.PlaySound(SoundEngine.SawtootWave, 130, 0.10f, TimeSpan.FromMilliseconds(2250));
            SoundEngine.PlaySound(SoundEngine.SawtootWave, 65, 0.10f, TimeSpan.FromMilliseconds(2250));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 261, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 261, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 261, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 261, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 587, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 587, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 440, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 440, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.FadeOutWhiteNoiseWave, 500, 0.35f, TimeSpan.FromMilliseconds(500));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 440, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 440, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 415, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 415, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 392, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 392, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.FadeOutWhiteNoiseWave, 500, 0.35f, TimeSpan.FromMilliseconds(500));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 349, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 349, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 293, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 293, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 349, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 349, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 392, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 392, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.FadeOutWhiteNoiseWave, 500, 0.35f, TimeSpan.FromMilliseconds(500));
            SoundEngine.PlaySound(SoundEngine.SawtootWave, 61, 0.10f, TimeSpan.FromMilliseconds(2250));
            SoundEngine.PlaySound(SoundEngine.SawtootWave, 61, 0.10f, TimeSpan.FromMilliseconds(2250));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 246, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 246, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 246, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 246, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 587, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 587, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 440, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 440, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.FadeOutWhiteNoiseWave, 500, 0.35f, TimeSpan.FromMilliseconds(500));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 440, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 440, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 415, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 415, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 392, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 392, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.FadeOutWhiteNoiseWave, 500, 0.35f, TimeSpan.FromMilliseconds(500));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 349, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 349, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 293, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 293, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 349, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 349, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 392, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 392, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.FadeOutWhiteNoiseWave, 500, 0.35f, TimeSpan.FromMilliseconds(500));
            SoundEngine.PlaySound(SoundEngine.SawtootWave, 58, 0.10f, TimeSpan.FromMilliseconds(2250));
            SoundEngine.PlaySound(SoundEngine.SawtootWave, 58, 0.10f, TimeSpan.FromMilliseconds(2250));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 233, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 233, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 233, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 233, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 587, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 587, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 440, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 440, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.FadeOutWhiteNoiseWave, 500, 0.35f, TimeSpan.FromMilliseconds(500));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 440, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 440, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 415, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 415, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 392, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 392, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.FadeOutWhiteNoiseWave, 500, 0.35f, TimeSpan.FromMilliseconds(500));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 349, 0.20f, TimeSpan.FromMilliseconds(250));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 349, 0.25f, TimeSpan.FromMilliseconds(250));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 293, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 293, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 349, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 349, 0.25f, TimeSpan.FromMilliseconds(125));
            SoundEngine.PlaySound(SoundEngine.TriangleWave, 392, 0.20f, TimeSpan.FromMilliseconds(125));
            await SoundEngine.PlaySound(SoundEngine.SineWave, 392, 0.25f, TimeSpan.FromMilliseconds(125));
        }

        public async static void ErrorSound()
        {
            await SoundEngine.PlaySound(SoundEngine.SineWave, 400, 0.25f, TimeSpan.FromMilliseconds(80));
            await SoundEngine.PlaySound(SoundEngine.SquareWave, 200, 0.25f, TimeSpan.FromMilliseconds(80));
        }
    }
}
