using CaModLoaderAPI;
using ConsoleAdventure.CaModLoaderAPI;
using ConsoleAdventure.Content.Scripts.Audio;
using ConsoleAdventure.Content.Scripts.InputLogic;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;


namespace ConsoleAdventure.Content.Scripts.UI
{
    public class Menu
    {
        public MenuState State;

        internal MenuButton[] menuButtons = new MenuButton[4];

        internal MenuButton[] menuSettingsButtons = new MenuButton[3];

        private List<WorldPanel> worldPanels = new List<WorldPanel>();

        private InfoPanel aboutGamePanel = null;

        private InfoPanel modDescription = null;

        private TextListUI modDescriptionList = null;

        private InfoPanel serverNotFoundPanel = null;

        private TextInputField[] worldGenTextFields = new TextInputField[2];

        private TextInputField[] modTextFields = new TextInputField[2];

        public int serverNotFoundTimer = 0;

        private static int worldDrawBuffer = 6;

        private int startWList, endWList = worldDrawBuffer;

        private int selectedLanguage = SettingsSystem.GetSetting("Options", "Language"); // Получить сохраненный язык

        private string modsListText = "";

        private int timer;

        public Menu()
        {
            MenuInit();
            WorldMenuInit();
        }

        public void MenuInit()
        {
            MainScreenInit();

            AboutGameInit();

            ModsPanelInit();

            SettingsInit();

            WorldLoadingProgressInit();

            ServerNotFoundPanelInit();

            WorldGenMenuInit();

            ControlConfigInit();

            ModCreateMenuInit();
        }

        public async void MenuUpdate()
        {
            MainScreenUpdate();

            WorldMenuUpdate();

            SettingsUpdate();

            ModsPanelUpdate();

            ServerNotFoundPanelUpdate();

            WorldGenMenuUpdate();

            ControlConfigUpdate();

            ModCreateMenuUpdate();

            ModDescriptionUpdate();

            if (Input.IsKeyDown(InputConfig.NavigationBeck) && timer >= Utils.StabilizeTicks(20))
            {
                bool modsOpened = (State == MenuState.mods);

                CloseAllPages();
                timer = 0;

                if (modsOpened)
                {
                    await CaModLoader.ReloadMods();
                    CaModLoader.SaveEnabledMods();
                    ModsPanelInit();
                }
            }

            if (State == MenuState.mainScreen || State == MenuState.settings) // Buttons Sound Play
            {
                if (Input.OnClick(InputConfig.NavigationLeft) ||
                    Input.OnClick(InputConfig.NavigationRight) ||
                    Input.OnClick(InputConfig.NavigationUp) ||
                    Input.OnClick(InputConfig.NavigationDown))
                {
                    TickSound();
                }
            }
            timer++;
        }

        #region MainScreen
        //FormatString fs = new("Форматированый текст [color:ff0000=\"Вау\"][color:00ff00=\"!\"]\nКрута! [color:0000ff=\"Вау!!\"] [color:23d8d1=\"∑\"] [item:ConsoleAdventure.Apple][item:ConsoleAdventure.TorchItem][item:ConsoleAdventure.IronPick][item:ConsoleAdventure.RubyItem][item:ConsoleAdventure.FurnaceItem]", new(), Color.White);

        FormatString fs = new(" [color:999999=\"T\"][color:dddddd=\"h\"][color:999999=\"e\"] [color:999999=\"Co\"][color:dddddd=\"nso\"][color:999999=\"le\"] [color:999999=\"Ad\"][color:dddddd=\"ventu\"][color:999999=\"re\"] [item:ConsoleAdventure.IronPick] ", new(), Color.White);
        private void MainScreenInit()
        {
            byte[] menuButtonTypes = new byte[4] { 0, 1, 3, 2 };

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
                    if (State > 0)
                        menuButtons[i].cursorColor = Color.Red;
                    menuButtons[i].isHover = true;
                }
            }

            if (curentMenuBatton == -1)
                menuButtons[0].isHover = true;
        }

        private void MainScreenUpdate()
        {
            if (State == MenuState.mainScreen) {
                for (int i = 0; i < menuButtons.Length; i++)
                {
                    int waitTime = Utils.StabilizeTicks(20);

                    if (Input.IsKeyDown(InputConfig.NavigationRight)&& timer >= waitTime) //прокрутка 
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

                    if (Input.IsKeyDown(InputConfig.NavigationLeft) && timer >= waitTime)
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

                    if (menuButtons[i].isHover && Input.IsKeyDown(InputConfig.NavigationSelect))
                    {
                        if (menuButtons[i].type == 0)
                        {
                            State = MenuState.worldMenu;
                            ConsoleAdventure.display = new Display(ConsoleAdventure.world);
                            menuButtons[i].cursorColor = Color.Red;
                            timer = 0;
                        }
                        if (menuButtons[i].type == 1)
                        {
                            menuButtons[i].cursorColor = Color.Red;
                            State = MenuState.settings;
                            timer = 0;
                        }
                        if (menuButtons[i].type == 3)
                        {
                            State = MenuState.mods;
                            timer = 0;
                        }
                        if (menuButtons[i].type == 2)
                        {
                            ConsoleAdventure.isExit = true;
                        }
                    }
                }
            }

            if (Input.PostClick(InputConfig.OpenLogs) && State == MenuState.mainScreen)
            {
                string logPath = Program.savePath + "Logs\\";
                if (Directory.Exists(logPath))
                    Utils.OpenExplorerAtFolder(logPath);
            }
        }

        private void MainScreenDraw(SpriteBatch spriteBatch)
        {
            for (int i = 0; i < menuButtons.Length; i++)
            {
                menuButtons[i].Draw(spriteBatch);
            }
            fs.Draw(spriteBatch);
            //ModPanel modPanel = new ModPanel(new(), "Mod", "Void Defi", "0.01");
            //modPanel.Draw(spriteBatch);
        }
        #endregion

        #region WorldMenu
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

        private void WorldMenuUpdate()
        {
            if (State == MenuState.worldMenu)
            {
                if (worldPanels.Count > 0)
                {
                    for (int i = 0; i < worldPanels.Count; i++)
                    {
                        int waitTime = Utils.StabilizeTicks(10);

                        if (Input.IsKeyDown(InputConfig.NavigationUp) && timer >= waitTime && (i - 1) != -1) //прокрутка
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

                        if (Input.IsKeyDown(InputConfig.NavigationDown) && timer >= waitTime && i < worldPanels.Count - 1)
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

                        if (Input.IsKeyDown(InputConfig.NavigationSelect) && timer >= 30) //теперь, тут не как по другому
                        {
                            if (worldPanels[i].curssor == 0 && worldPanels[i].isHover)
                            {
                                ConsoleAdventure.progressBar.stepText = Localization.GetTranslation("Progress", "LoadFile");
                                ConsoleAdventure.progressBar.Progress = 0;

                                string name = worldPanels[i].name;
                                State = MenuState.worldLoadingProgress;

                                Display.SetBars();

                                Thread load = new Thread(new ThreadStart(LoadWorld));
                                load.Start();

                                timer = 0;

                                void LoadWorld()
                                {
                                    bool inm = false; // in multiplayer
                                    bool ish = false; // is host
                                    if (ConsoleAdventure.kstate.IsKeyDown(Keys.M)) inm = true;
                                    if (ConsoleAdventure.kstate.IsKeyDown(Keys.H)) { ish = true; inm = true; }
                                    NetworkManager.isHost = ish;
                                    ConsoleAdventure.CreateWorld(name, 1234, false, inm);
                                    if (ConsoleAdventure.world != null) WorldIO.Load(name);
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

                    if (Input.IsKeyDown(InputConfig.NavigationLeft) && timer >= Utils.StabilizeTicks(15)) //прокрутка кнопок мира
                    {
                        for (int i = 0; i < worldPanels.Count; i++)
                        {
                            worldPanels[i].curssor = (worldPanels[i].curssor - 1 + 3) % 3;
                        }
                        timer = 0;
                    }

                    if (Input.IsKeyDown(InputConfig.NavigationRight) && timer >= Utils.StabilizeTicks(15))
                    {
                        for (int i = 0; i < worldPanels.Count; i++)
                        {
                            worldPanels[i].curssor = (worldPanels[i].curssor + 1) % 3;
                        }
                        timer = 0;
                    }
                }

                if (Input.PostClick(InputConfig.WorldGen))
                {
                    State = MenuState.wordGenMenu;
                }
            }
        }

        private void WorldMenuDraw(SpriteBatch spriteBatch)
        {
            if (State == MenuState.worldMenu)
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

                    string navigHelpWorld = TextAssets.navigHelpWorld.Replace("[1]", Localization.GetTranslation("Keys", InputConfig.NavigationSelect.key.ToString()));
                    spriteBatch.DrawString(ConsoleAdventure.Font, navigHelpWorld, new Vector2(ConsoleAdventure.Width - (ConsoleAdventure.Font.MeasureString(navigHelpWorld).X + 120) + 9 * 12, ConsoleAdventure.Height - 114), Color.Gray);
                }

                else
                {
                    string hwc = TextAssets.HelpWorldCreate.Replace("[1]", Localization.GetTranslation("Keys", InputConfig.WorldGen.key.ToString()));
                    spriteBatch.DrawString(ConsoleAdventure.Font, hwc, new Vector2(ConsoleAdventure.Width / 2 - (ConsoleAdventure.Font.MeasureString(hwc).X / 2), ConsoleAdventure.Height / 2 - 180), Color.Gray);
                }
            }
        }
        #endregion

        #region Settings
        private void SettingsInit()
        {
            byte[] menuSettingsButtonTypes = new byte[3] { 0, 1, 2 };

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

                if (menuSettingsButtonTypes[i] == 0)
                {
                    menuSettingsButtons[i].text += Localization.GetLanguageName(selectedLanguage);
                    menuSettingsButtons[i].Position = SettingButtonPos;
                    menuSettingsButtons[i].Center = SettingButtonPos;
                }
            }

            menuSettingsButtons[0].isHover = true;
        }

        private void SettingsUpdate()
        {
            if (State == MenuState.settings)
            {
                for (int i = 0; i < menuSettingsButtons.Length; i++)
                {
                    int waitTime = Utils.StabilizeTicks(10);

                    if (menuSettingsButtons[i].isHover && i == 0)
                    {

                        if (Input.IsKeyDown(InputConfig.NavigationRight) && timer >= waitTime)
                        {
                            if (selectedLanguage < 1)
                                selectedLanguage += 1;

                            ReLocalize();
                            menuSettingsButtons[i].text += Localization.GetLanguageName(selectedLanguage);

                            timer = 0;
                        }
                        if (Input.IsKeyDown(InputConfig.NavigationLeft) && timer >= waitTime)
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

                    if (Input.IsKeyDown(InputConfig.NavigationUp) && timer >= waitTime)
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

                    if (Input.IsKeyDown(InputConfig.NavigationDown) && timer >= waitTime)
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

                    if (menuSettingsButtons[i].isHover && Input.IsKeyDown(InputConfig.NavigationSelect) && timer >= Utils.StabilizeTicks(30))
                    {
                        if (menuSettingsButtons[i].type == 1)
                        {
                            State = MenuState.aboutGame;
                        }

                        if (menuSettingsButtons[i].type == 2)
                        {
                            State = MenuState.aboutControl;
                        }
                        timer = 0;
                    }
                }

                if (Input.IsKeyDown(InputConfig.NavigationLeft) && timer >= Utils.StabilizeTicks(15)) //прокрутка кнопок мира
                {
                    for (int i = 0; i < worldPanels.Count; i++)
                    {
                        worldPanels[i].curssor = (worldPanels[i].curssor - 1 + 3) % 3;
                    }
                    timer = 0;
                }

                if (Input.IsKeyDown(InputConfig.NavigationRight) && timer >= Utils.StabilizeTicks(15))
                {
                    for (int i = 0; i < worldPanels.Count; i++)
                    {
                        worldPanels[i].curssor = (worldPanels[i].curssor + 1) % 3;
                    }
                    timer = 0;
                }
            }
        }

        private void SettingsDraw(SpriteBatch spriteBatch)
        {
            if (State == MenuState.settings || State == MenuState.aboutGame)
            {
                for (int i = 0; i < menuSettingsButtons.Length; i++)
                {
                    menuSettingsButtons[i].Draw(spriteBatch);
                }
            }
        }
        #endregion

        #region AboutGame
        private void AboutGameInit()
        {
            aboutGamePanel = new InfoPanel(new Rectangle((ConsoleAdventure.screenWidth / 2) - 28 * 9, (ConsoleAdventure.screenHeight / 2) - 20 * 18, 64, 30), TextAssets.About, TextAssets.AboutGame);
        }

        private void AboutGameDraw(SpriteBatch spriteBatch)
        {
            if (State == MenuState.aboutGame)
            {
                aboutGamePanel.Draw(spriteBatch);
            }
        }
        #endregion

        #region ModsPanel

        ModList modList = null;

        private void ModsPanelInit()
        {
            List<BaseUI> mods = new();

            for (int i = 0; i < CaModLoader.allMods.Count; i++)
            {
                IMod modData = CaModLoader.allMods[i];
                mods.Add(new ModPanel(new(0, 0, 0, 0), modData));
            }

            modList = new("", new(ConsoleAdventure.Width / 2.78f, ConsoleAdventure.Height / 4 + 50), mods, Color.White);
            if (modList.elements.Count > 0)
                modList.elements[0].isHover = true;
            modList.drawBuffer = 5;
            modList.endList = 5;
        }
        
        int modsTimer;

        private void ModsPanelUpdate()
        {
            if (State == MenuState.mods)
            {
                if (Input.IsKeyDown(InputConfig.OpenLogs) && timer >= Utils.StabilizeTicks(30))
                {
                    Utils.OpenExplorerAtFolder(AppDomain.CurrentDomain.BaseDirectory + "Content\\mods\\");
                    timer = 0;
                }

                if (Input.PostClick(InputConfig.ModCreate))
                {
                    State = MenuState.modCreateMenu;
                }

                modList.Update(ref modsTimer);
                modList.isHover = true;
                modsTimer++;
            }

            else
            {
                modList.isHover = false;
                modsTimer = 0;
            }
        }     

        private void ModsPanelDraw(SpriteBatch spriteBatch)
        {
            if (State == MenuState.mods)
            {
                //modListPanel.Draw(spriteBatch);
                string ModNavHelp = Localization.GetTranslation("UI", "NavigationMod").Replace("[1]", Localization.GetTranslation("Keys", InputConfig.NavigationLeft.key.ToString())).Replace("[2]", Localization.GetTranslation("Keys", InputConfig.NavigationRight.key.ToString()));
                spriteBatch.DrawString(ConsoleAdventure.Font, ModNavHelp, new Vector2(ConsoleAdventure.Width - (ConsoleAdventure.Font.MeasureString(ModNavHelp).X + 10), ConsoleAdventure.Height - 125), Color.Gray);
                string ModNavHelp2 = Localization.GetTranslation("UI", "NavigationMod2").Replace("[1]", Localization.GetTranslation("Keys", InputConfig.NavigationSelect.key.ToString()));
                spriteBatch.DrawString(ConsoleAdventure.Font, ModNavHelp2, new Vector2(ConsoleAdventure.Width - (ConsoleAdventure.Font.MeasureString(ModNavHelp2).X + 10), ConsoleAdventure.Height - 175), Color.Gray);

                string navModHelp = TextAssets.navigModFolderHelp.Replace("[1]", Localization.GetTranslation("Keys", InputConfig.OpenLogs.key.ToString()));
                spriteBatch.DrawString(ConsoleAdventure.Font, navModHelp, new Vector2(ConsoleAdventure.Width - (ConsoleAdventure.Font.MeasureString(navModHelp).X + 10), ConsoleAdventure.Height - 100), Color.Gray);
                string ModNewHelp = TextAssets.modCreateHelp.Replace("[1]", Localization.GetTranslation("Keys", InputConfig.WorldGen.key.ToString()));
                spriteBatch.DrawString(ConsoleAdventure.Font, ModNewHelp, new Vector2(ConsoleAdventure.Width - (ConsoleAdventure.Font.MeasureString(ModNewHelp).X + 10), ConsoleAdventure.Height - 75), Color.Gray);

                modList.Draw(spriteBatch);       
            }    
        }

        internal void OpenModDescription(string text)
        {
            modDescriptionList = new(text, 40, new(ConsoleAdventure.Width / 2 - (21 * 9), ConsoleAdventure.Height / 3), Color.White);
            modDescriptionList.drawBuffer = 20;
            modDescriptionList.startList = 0;
            modDescriptionList.endList = 20;
            modDescriptionList.height = 19;
            State = MenuState.modDescription;
        }

        internal void CloseModDescription()
        {
            modDescriptionList = null;
            State = MenuState.mods;
        }

        
        private void ModDescriptionDraw(SpriteBatch spriteBatch)
        {
            if (modDescriptionList != null && State == MenuState.modDescription)
            {
                modDescriptionList.Draw(spriteBatch);

                float sliderY = (((float)modDescriptionList.startList / (float)(modDescriptionList.elements.Count - 21)) * 19f);
                Vector2 sliderPos = new Vector2(modDescriptionList.Position.X + (42 * 9), modDescriptionList.Position.Y + sliderY * 19);

                spriteBatch.DrawString(ConsoleAdventure.Font, "▴", new(sliderPos.X + 2, modDescriptionList.Position.Y - 15), Color.White);
                spriteBatch.DrawString(ConsoleAdventure.Font, "█", sliderPos, Color.White);
                spriteBatch.DrawString(ConsoleAdventure.Font, "▾", new(sliderPos.X + 2, modDescriptionList.Position.Y + 5 + 19.5f * 19), Color.White);
            }
        }

        private void ModDescriptionUpdate()
        {
            if (modDescriptionList != null && State == MenuState.modDescription)
            {
                modDescriptionList.Update(ref timer);
   

                if (Input.PostClick(InputConfig.NavigationBeck))
                {
                    CloseModDescription();
                }
            }
        }

        #endregion

        #region WorldLoadingProgress
        private void WorldLoadingProgressInit()
        {
            ConsoleAdventure.progressBar = new ProgressBar(new Rectangle(new Point((int)ConsoleAdventure.Width / 2, ((int)ConsoleAdventure.Height / 2) - 120), new Point(50 * 9, 19)), Color.LightGreen, 50, ProgressBar.PercentRight);
        }

        private void WorldLoadingProgressDraw(SpriteBatch spriteBatch)
        {
            if (State == MenuState.worldLoadingProgress)
            {
                ConsoleAdventure.progressBar.Draw(spriteBatch);
            }
        }
        #endregion

        #region ServerNotFoundPanel
        private void ServerNotFoundPanelInit()
        {
            serverNotFoundPanel = new InfoPanel(new Rectangle(ConsoleAdventure.screenWidth / 2 - 135, ConsoleAdventure.screenHeight / 2 - 57, 30, 6), "ServerNotFound", "Server Not Found");
        }
        private void ServerNotFoundPanelUpdate()
        {
            if (serverNotFoundTimer > 0)
                serverNotFoundTimer--;
        }
        private void ServerNotFoundPanelDraw(SpriteBatch spriteBatch)
        {
            if (serverNotFoundTimer > 0)
                serverNotFoundPanel.Draw(spriteBatch);
        }
        #endregion

        #region WordGenMenu

        int WGErrorType = -1;

        private void WorldGenMenuInit()
        {
            byte[] textFieldTypes = new byte[2] { 0, 1 };

            for (int i = 0; i < textFieldTypes.Length; i++)
            {
                string text = "";
                char[] chars = null;
                bool charsType = false;

                switch (textFieldTypes[i])
                {
                    case 0:
                        text = Localization.GetTranslation("UI", "WorldNameField");
                        chars = new char[] {'\\', '|', '/','<', '>', '*', '"', '?', '\n', '\r' };
                        charsType = TextInput.BlackList;
                        break;
                    case 1:
                        text = Localization.GetTranslation("UI", "WorldSeedField");
                        chars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                        charsType = TextInput.WhiteList;
                        break;
                    case 2:
                        text = Localization.GetTranslation("UI", "WorldSizeField");
                        chars = new char[] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' };
                        charsType = TextInput.WhiteList;
                        break;
                    case 3:
                        text = Localization.GetTranslation("UI", "WorldPlayerNameField");
                        chars = new char[] { '\n', '\r' };
                        charsType = TextInput.BlackList;
                        break;
                }
                int startPos = 400;
                int indent = 20;

                worldGenTextFields[i] = new TextInputField(new Point((int)(ConsoleAdventure.Width / 2), startPos + (int)(indent * (i - 1.5f))), new Color(255, 255, 255), 15, 1, text, 0, chars, charsType);
            }

            worldGenTextFields[0].isHover = true;
        }

        private void WorldGenMenuDraw(SpriteBatch spriteBatch)
        {
            if (State == MenuState.wordGenMenu)
            {
                for (int i = 0; i < worldGenTextFields.Length; i++)
                {
                    worldGenTextFields[i].Draw(spriteBatch);
                }
            }
        }

        private void WorldGenMenuUpdate()
        {
            if (State == MenuState.wordGenMenu)
            {
                ConsoleAdventure.BlockHotKey = true;
                for (int i = 0; i < worldGenTextFields.Length; i++)
                {
                    int waitTime = Utils.StabilizeTicks(20);

                    if (Input.IsKeyDown(InputConfig.NavigationDown) && timer >= waitTime) //прокрутка 
                    {
                        if (worldGenTextFields[i].isHover)
                        {
                            worldGenTextFields[i].isHover = false;

                            if (i != worldGenTextFields.Length - 1) //перемещяем курсор
                                worldGenTextFields[i + 1].isHover = true;
                            else
                                worldGenTextFields[0].isHover = true;

                            timer = 0;
                        }
                    }

                    if (Input.IsKeyDown(InputConfig.NavigationUp) && timer >= waitTime)
                    {
                        if (worldGenTextFields[i].isHover)
                        {
                            worldGenTextFields[i].isHover = false;

                            if (i != 0)
                                worldGenTextFields[i - 1].isHover = true;
                            else
                                worldGenTextFields[worldGenTextFields.Length - 1].isHover = true;

                            timer = 0;
                        }
                    }
                }

                if (Input.IsKeyDown(InputConfig.NavigationBeck))
                {
                    ConsoleAdventure.BlockHotKey = false;
                }

                if (Input.IsKeyDown(InputConfig.NavigationSelect))
                {
                    //Thread gen = new Thread(new ThreadStart(Gen));
                    //gen.Start();

                    //void Gen()
                    //{
                    if (WGErrorType == -1)
                    {
                        string name = worldGenTextFields[0].text;
                        string[] worlds = WorldIO.GetWorlds(false).names;

                        int countIdenticalWorldName = 0;
                        for (int i = 0; i < worlds.Length; i++)
                        {   string curEndName = (countIdenticalWorldName > 0 ? countIdenticalWorldName.ToString() : "");
                            if (worlds[i] == name + curEndName)
                            {
                                countIdenticalWorldName++;
                            }
                        }

                        if(countIdenticalWorldName > 0)
                        {
                            name += countIdenticalWorldName;
                        }
                        
                        ConsoleAdventure.CreateWorld(name, int.Parse(worldGenTextFields[1].text)); //"World" + (worldPanels.Count > 0 ? worldPanels.Count : ""), ConsoleAdventure.rand.Next(0, 100000000)
                        WorldIO.Save(ConsoleAdventure.world.name);
                        worldPanels.Clear();
                        ConsoleAdventure.BlockHotKey = false;
                        WorldMenuInit();
                    }

                    else
                    {
                        return;
                    }
                    //}

                    for (int i = 0; i < worldGenTextFields.Length; i++)
                    {
                        worldGenTextFields[i].text = "";
                        worldGenTextFields[i].cursorPos = new();
                        worldGenTextFields[i].isHover = false;
                        worldGenTextFields[i].color = Color.White;
                        WGErrorType = -1;
                    }

                    worldGenTextFields[0].isHover = true;

                    State = MenuState.worldMenu;
                }

                for (int i = 0; i < worldGenTextFields.Length; i++)
                {
                    worldGenTextFields[i].Update();
                }

                if (worldGenTextFields[1].text != "" && !int.TryParse(worldGenTextFields[1].text, out int r0)) { worldGenTextFields[1].color = Color.Red; WGErrorType = 0; }
                else { worldGenTextFields[1].color = Color.White; WGErrorType = -1; }

                if(worldGenTextFields[1].text == "") WGErrorType = 0;

                //if (worldGenTextFields[2].text != "" && !int.TryParse(worldGenTextFields[2].text, out int r1)) worldGenTextFields[2].color = Color.Red;
                //else worldGenTextFields[2].color = Color.White;
            }
        }
        #endregion

        #region ControlConfig

        ControlConfig controlConfig = null;

        private void ControlConfigInit()
        {
            List<BaseUI> keys = new();

            for (int i = 0; i < InputConfig.AllKeys.Count; i++)
            {
                keys.Add(new KeyPanel(new(0, 0, 80 * 9, 4 * 19), InputConfig.AllKeys[i]));
            }

            controlConfig = new("", new(ConsoleAdventure.Width / 3.5f, ConsoleAdventure.Height / 4 + 50), keys, Color.White);
            controlConfig.elements[0].isHover = true;
            controlConfig.drawBuffer = 10;
            controlConfig.endList = 10;
        }

        int controlTimer;
        private void ControlConfigUpdate()
        {
            if (State == MenuState.aboutControl)
            {
                if (Input.PostClick(InputConfig.ControlEdit) && controlTimer > 60)
                {
                    for(int i = 0; i < controlConfig.elements.Count; i++)
                    {
                        if (controlConfig.elements[i].isHover)
                        {
                            if (!((KeyPanel)controlConfig.elements[i]).flag) ((KeyPanel)controlConfig.elements[i]).flag = true;
                            else if (((KeyPanel)controlConfig.elements[i]).flag) ((KeyPanel)controlConfig.elements[i]).flag = false;
                            controlTimer = 0;
                        }
                    }
                }

                if (Input.PostClick(InputConfig.ControlReset) && controlTimer > 60)
                {
                    for (int i = 0; i < controlConfig.elements.Count; i++)
                    {
                        if (controlConfig.elements[i].isHover)
                        {
                            ((KeyPanel)controlConfig.elements[i]).key.key = Keys.None;
                        }
                    }
                }

                controlConfig.Update(ref controlTimer);
                controlConfig.isHover = true;
            }

            if (State != MenuState.aboutControl)
                controlConfig.isHover = false;

            controlTimer++;
        }

        private void ControlConfigDraw(SpriteBatch spriteBatch)
        {
            if (State == MenuState.aboutControl)
            {
                controlConfig.Draw(spriteBatch);

                string navControl1Help = TextAssets.navigHelpControl1.Replace("[1]", Localization.GetTranslation("Keys", InputConfig.ControlEdit.key.ToString()));
                spriteBatch.DrawString(ConsoleAdventure.Font, navControl1Help, new Vector2(ConsoleAdventure.Width - (ConsoleAdventure.Font.MeasureString(navControl1Help).X + 10), ConsoleAdventure.Height - 100), Color.Gray);
                string navControl2Help = TextAssets.navigHelpControl2.Replace("[1]", Localization.GetTranslation("Keys", InputConfig.ControlReset.key.ToString()));
                spriteBatch.DrawString(ConsoleAdventure.Font, navControl2Help, new Vector2(ConsoleAdventure.Width - (ConsoleAdventure.Font.MeasureString(navControl2Help).X + 10), ConsoleAdventure.Height - 75), Color.Gray);

            }
        }

        #endregion

        #region ModCreateMenu

        int MCErrorType = -1;

        private void ModCreateMenuInit()
        {
            byte[] textFieldTypes = new byte[2] { 0, 1 };

            for (int i = 0; i < textFieldTypes.Length; i++)
            {
                string text = "";
                char[] chars = new char[] { 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', 'a', 's', 'd', 
                                            'f', 'g', 'h', 'j', 'k', 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm', 
                                            'Q', 'W', 'E', 'R', 'T', 'Y', 'U', 'I', 'O', 'P', 'A', 'S', 'D',
                                            'F', 'G', 'H', 'J', 'K', 'L', 'Z', 'X', 'C', 'V', 'B', 'N', 'M' };

                bool charsType = TextInput.WhiteList;

                switch (textFieldTypes[i])
                {
                    case 0:
                        text = Localization.GetTranslation("UI", "ModNameField");
                        break;
                    case 1:
                        text = Localization.GetTranslation("UI", "ModAuthorField");
                        chars = new char[0];
                        charsType = false;
                        break;
                }
                int startPos = 400;
                int indent = 20;

                modTextFields[i] = new TextInputField(new Point((int)(ConsoleAdventure.Width / 2), startPos + (int)(indent * (i - 1.5f))), new Color(255, 255, 255), 15, 1, text, 0, chars, charsType);
            }

            modTextFields[0].isHover = true;
        }

        private void ModCreateMenuDraw(SpriteBatch spriteBatch)
        {
            if (State == MenuState.modCreateMenu)
            {
                for (int i = 0; i < modTextFields.Length; i++)
                {
                    modTextFields[i].Draw(spriteBatch);
                }
            }
        }

        private void ModCreateMenuUpdate()
        {
            if (State == MenuState.modCreateMenu)
            {
                ConsoleAdventure.BlockHotKey = true;
                for (int i = 0; i < modTextFields.Length; i++)
                {
                    int waitTime = Utils.StabilizeTicks(20);

                    if (Input.IsKeyDown(InputConfig.NavigationDown) && timer >= waitTime) //прокрутка 
                    {
                        if (modTextFields[i].isHover)
                        {
                            modTextFields[i].isHover = false;

                            if (i != modTextFields.Length - 1) //перемещяем курсор
                                modTextFields[i + 1].isHover = true;
                            else
                                modTextFields[0].isHover = true;

                            timer = 0;
                        }
                    }

                    if (Input.IsKeyDown(InputConfig.NavigationUp) && timer >= waitTime)
                    {
                        if (modTextFields[i].isHover)
                        {
                            modTextFields[i].isHover = false;

                            if (i != 0)
                                modTextFields[i - 1].isHover = true;
                            else
                                modTextFields[modTextFields.Length - 1].isHover = true;

                            timer = 0;
                        }
                    }
                }

                if (Input.IsKeyDown(InputConfig.NavigationBeck))
                {
                    ConsoleAdventure.BlockHotKey = false;
                }

                if (Input.IsKeyDown(InputConfig.NavigationSelect))
                {
                    if (MCErrorType == -1)
                    {
                        ModCreator.CreateMod(modTextFields[0].text, modTextFields[1].text);
                        ConsoleAdventure.BlockHotKey = false;      
                    }

                    else
                    {
                        return;
                    }

                    for (int i = 0; i < modTextFields.Length; i++)
                    {
                        modTextFields[i].text = "";
                        modTextFields[i].cursorPos = new();
                        modTextFields[i].isHover = false;
                        modTextFields[i].color = Color.White;
                        MCErrorType = -1;
                    }

                    modTextFields[0].isHover = true;

                    State = MenuState.mods;
                }

                for (int i = 0; i < modTextFields.Length; i++)
                {
                    modTextFields[i].Update();
                }

                if (modTextFields[0].text == "") { MCErrorType = 0; }
                else if (modTextFields[1].text == "") { MCErrorType = 0; }
                else { MCErrorType = -1; }
            }
        }
        #endregion

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
            // Draw Logo
            spriteBatch.DrawString(ConsoleAdventure.Font, TextAssets.logo, new Vector2((ConsoleAdventure.Width / 2) - (621 / 2), 20), Color.White);
            // Draw Version
            spriteBatch.DrawString(ConsoleAdventure.Font, Docs.GetInfo(), new Vector2(10, ConsoleAdventure.Height - 25), Color.White);
            // Draw navigation help
            string navHelp = TextAssets.navigHelp.Replace("[1]", Localization.GetTranslation("Keys", InputConfig.NavigationLeft.key.ToString()))
                                                 .Replace("[2]", Localization.GetTranslation("Keys", InputConfig.NavigationRight.key.ToString()))
                                                 .Replace("[3]", Localization.GetTranslation("Keys", InputConfig.NavigationUp.key.ToString()))
                                                 .Replace("[4]", Localization.GetTranslation("Keys", InputConfig.NavigationDown.key.ToString()))
                                                 .Replace("[5]", Localization.GetTranslation("Keys", InputConfig.NavigationSelect.key.ToString()));
            spriteBatch.DrawString(ConsoleAdventure.Font, navHelp, new Vector2(ConsoleAdventure.Width - (ConsoleAdventure.Font.MeasureString(navHelp).X + 10), ConsoleAdventure.Height - 25), Color.Gray);
            
            if (State > 0)
            {
                // Draw Esc Help
                string navBeckHelp = TextAssets.navigHelpBack.Replace("[1]", Localization.GetTranslation("Keys", InputConfig.NavigationBeck.key.ToString()));
                spriteBatch.DrawString(ConsoleAdventure.Font, navBeckHelp, new Vector2(ConsoleAdventure.Width - (ConsoleAdventure.Font.MeasureString(navBeckHelp).X + 10), ConsoleAdventure.Height - 50), Color.Gray);
            }

            MainScreenDraw(spriteBatch);

            WorldMenuDraw(spriteBatch);

            SettingsDraw(spriteBatch);

            AboutGameDraw(spriteBatch);

            ModsPanelDraw(spriteBatch);

            WorldLoadingProgressDraw(spriteBatch);

            ServerNotFoundPanelDraw(spriteBatch);

            WorldGenMenuDraw(spriteBatch);

            ControlConfigDraw(spriteBatch);

            ModCreateMenuDraw(spriteBatch);

            ModDescriptionDraw(spriteBatch);
            
            spriteBatch.End();
        }

        public async static void TickSound()
        {
            await SoundEngine.PlaySound(SoundEngine.BubbleWave, 500, 0.1f, TimeSpan.FromMilliseconds(500));   
        }

        public async static void ErrorSound()
        {
            await SoundEngine.PlaySound(SoundEngine.SineWave, 400, 0.25f, TimeSpan.FromMilliseconds(80));
            await SoundEngine.PlaySound(SoundEngine.SquareWave, 200, 0.25f, TimeSpan.FromMilliseconds(80));
        }
    }
}
