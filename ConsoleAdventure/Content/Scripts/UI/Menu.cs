using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.Content.Scripts.UI
{
    public class Menu
    {
        public int State { get; private set; }

        private MenuButton[] menuButtons = new MenuButton[3];

        private MenuButton[] menuSettingsButtons = new MenuButton[3];

        private List<WorldPanel> worldPanels = new List<WorldPanel>();

        private InfoPanel aboutGamePanel = null;

        private InfoPanel aboutControlPanel = null;

        private static int worldDrawBuffer = 6;

        private int startWList, endWList = worldDrawBuffer;

        private int selectedLanguage = SettingsSystem.GetSetting("Options", "Language"); // Получить сохраненный язык

        public Menu()
        {
            MenuInit();
        }

        public void MenuInit()
        {
            byte[] menuButtonTypes = new byte[3] { 0, 1, 2 };

            byte[] menuSettingsButtonTypes = new byte[3] { 0, 1, 2 };

            aboutGamePanel = new InfoPanel(new Rectangle((ConsoleAdventure.screenWidth / 2) - 32 * 9, (ConsoleAdventure.screenHeight / 2) - 20 * 18, 64, 30), TextAssets.About, TextAssets.AboutGame);

            aboutControlPanel = new InfoPanel(new Rectangle((ConsoleAdventure.screenWidth / 2) - 32 * 9, (ConsoleAdventure.screenHeight / 2) - 20 * 18, 64, 30), TextAssets.Control, TextAssets.AboutControl);

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
                }
                int startPos = 220;
                int indent = 100;

                menuButtons[i] = new MenuButton(new Vector2((ConsoleAdventure.Width / 2) + (indent * (i - 1)), startPos), text, new Color(230, 230, 230), menuButtonTypes[i]);
            }

            menuButtons[0].isHover = true;

            menuSettingsButtons[0].isHover = true;

            for (int i = 0; i < 40; i++)
            {
                worldPanels.Add(new WorldPanel(new Rectangle(), $"World{i}", "1234"));
            }
            worldPanels[0].isHover = true;
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
                        if (menuButtons[i].type == 2)
                        {
                            ConsoleAdventure.isExit = true;
                        }
                    }
                }
            }

            if (State == 1)
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
                        if (worldPanels[i].curssor == 0)
                        {
                            ConsoleAdventure.CreateWorld();
                            Saves.Load("World");
                            ConsoleAdventure.InWorld = true;
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

            if (ConsoleAdventure.kstate.IsKeyDown(Keys.Escape) && timer >= Utils.StabilizeTicks(20))
            {
                State = 0;
                for (int i = 0; i < menuButtons.Length; i++)
                {
                    menuButtons[i].cursorColor = Color.Yellow;
                }
                timer = 0;
            }
            timer++;
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

            spriteBatch.End();
        }
    }
}
