using ConsoleAdventure.Content.Scripts.UI;
using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.IO;

using System.Linq;

namespace ConsoleAdventure
{
    public class ConsoleAdventure : Game
    {
        private GraphicsDeviceManager _graphics;
        public static SpriteBatch _spriteBatch;

        private static SpriteFont font;

        World world = new World();
        Display display;

        int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        public static Language language = Language.russian;

        public static bool InWorld = false;

        public static int screenWidth = 1602;
        public static int screenHeight = 912;

        private Color bg = Color.Black; //new Color(25, 25, 25);

        public static KeyboardState prekstate;
        public static KeyboardState kstate;

        public int State { get; private set; }

        private MenuButton[] menuButtons = new MenuButton[3];

        private List<WorldPanel> worldPanels = new List<WorldPanel>();

        private static int worldDrawBuffer = 6;

        private int startWList, endWList = worldDrawBuffer;

        public static SpriteFont Font => font;

        public ConsoleAdventure()
        {
            _graphics = new GraphicsDeviceManager(this);
            Localization.Load();
        }

        protected override void Initialize()
        {
            display = new Display(world);

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            IsFixedTimeStep = false;
            _graphics.SynchronizeWithVerticalRetrace = false;

            Window.Title = $"Console Adventure {Docs.version}. By Bonds";

            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
            _graphics.ApplyChanges();

            Window.AllowUserResizing = true;

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/font");

            byte[] menuButtonTypes = new byte[3] { 0, 1, 2 };
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

                menuButtons[i] = new MenuButton(Vector2.Zero, text, 1f, 1, new Color(230, 230, 230), menuButtonTypes[i]);
                menuButtons[i].Center = new Vector2((_graphics.PreferredBackBufferWidth / 2) + (indent * (i - 1)), startPos);
            }

            menuButtons[0].isHover = true;

            for (int i = 0; i < 40; i++)
            {
                worldPanels.Add(new(Vector2.Zero, "──────────────────────────────────────────────", Color.White, $"World{i}", "1234"));
            }
            worldPanels[0].isHover = true;
        }

        protected override void Update(GameTime gameTime)
        {
            prekstate = kstate;
            kstate = Keyboard.GetState();

            elapsedTime += gameTime.ElapsedGameTime;
            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }
            frameCounter++;


            if (InWorld)
            {
                world.ListenEvents();

                if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape))
                {
                    InWorld = false;
                }
            }       

            else
            {
                MenuUpdate();
            }
            
            base.Update(gameTime);
        }

        int timer;

        public void MenuUpdate()
        {
            for (int i = 0; i < menuButtons.Length; i++)
            {
                int waitTime = 20 * (frameRate / 60);

                if (State == 0)
                {
                    if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right) && timer >= waitTime)
                    {
                        if (menuButtons[i].isHover)
                        {
                            menuButtons[i].isHover = false;

                            if (i != menuButtons.Length - 1)
                                menuButtons[i + 1].isHover = true;
                            else
                                menuButtons[0].isHover = true;

                            timer = 0;
                        }
                    }

                    if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left) && timer >= waitTime)
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

                    if (menuButtons[i].isHover && kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter))
                    {
                        if (menuButtons[i].type == 0)
                        {
                            State = 1;
                            display = new Display(world);
                            menuButtons[i].cursorColor = Color.Red;
                            timer = 0;
                        }

                        if (menuButtons[i].type == 2)
                        {
                            Exit();
                        }
                    }
                }
            }

            if (State == 1)
            {
                for (int i = 0; i < worldPanels.Count; i++)
                {
                    int waitTime = 10 * (frameRate / 60);

                    if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up) && timer >= waitTime && (i - 1) != -1)
                    {
                        if (worldPanels[i].isHover)
                        {
                            worldPanels[i].isHover = false;
                            int newIndex = (i - 1 + worldPanels.Count) % worldPanels.Count;
                            worldPanels[newIndex].isHover = true;

                            if (newIndex < startWList)
                            {
                                startWList = newIndex;
                                endWList = Math.Min(startWList + worldDrawBuffer, worldPanels.Count);
                            }

                            timer = 0;
                            break;
                        }
                    }

                    if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down) && timer >= waitTime && i < worldPanels.Count - 1)
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

                    if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Enter) && timer >= 30 * (frameRate / 60))
                    {
                        if (worldPanels[i].curssor == 0)
                        {
                            ConsoleAdventure.InWorld = true;
                        }
                        timer = 0;
                    }
                }

                if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Left) && timer >= 15 * (frameRate / 60))
                {
                    for (int i = 0; i < worldPanels.Count; i++)
                    {
                        worldPanels[i].curssor = (worldPanels[i].curssor - 1 + 3) % 3;
                    }
                    timer = 0;
                }

                if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Right) && timer >= 15 * (frameRate / 60))
                {
                    for (int i = 0; i < worldPanels.Count; i++)
                    {
                        worldPanels[i].curssor = (worldPanels[i].curssor + 1) % 3;
                    }
                    timer = 0;
                }
            }

            if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Escape) && timer >= 20 * (frameRate / 60))
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

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(bg);

            if (InWorld)
            {
                _spriteBatch.Begin();

                _spriteBatch.DrawString(font, $"FPS: {(int)frameRate}", new Vector2(10, _graphics.PreferredBackBufferHeight - 30), Color.White);

                _spriteBatch.DrawString(font, display.DisplayInfo(), new Vector2(10, 10), Color.Gray);
                _spriteBatch.DrawString(font, display.DisplayInventory(), new Vector2(_graphics.PreferredBackBufferWidth - 300, 10), Color.White);
                display.DrawWorld();

                _spriteBatch.End();
            }

            else
            {
                _spriteBatch.Begin(samplerState: SamplerState.PointClamp);
                _spriteBatch.DrawString(font, TextAssets.logo, new Vector2((_graphics.PreferredBackBufferWidth / 2) - (621 / 2), 20), Color.White);
                _spriteBatch.DrawString(font, Docs.GetInfo(), new Vector2(10, _graphics.PreferredBackBufferHeight - 25), Color.White);
                _spriteBatch.DrawString(font, TextAssets.navigHelp, new Vector2(_graphics.PreferredBackBufferWidth - (font.MeasureString(TextAssets.navigHelp).X + 10), _graphics.PreferredBackBufferHeight - 25), Color.Gray);

                if (State > 0)
                {
                    _spriteBatch.DrawString(font, TextAssets.navigHelpBack, new Vector2(_graphics.PreferredBackBufferWidth - (font.MeasureString(TextAssets.navigHelpBack).X + 10), _graphics.PreferredBackBufferHeight - 50), Color.Gray);
                }

                for (int i = 0; i < menuButtons.Length; i++)
                {
                    menuButtons[i].Draw(_spriteBatch);
                }

                if (State == 1)
                {
                    int number = 0;
                    for (int i = startWList; i < endWList; i++)
                    {
                        if (i < worldPanels.Count) // Ensure index is within bounds
                        {
                            worldPanels[i].Center = new Vector2((_graphics.PreferredBackBufferWidth / 2) - 207, (number * (19 * 4)) + 9 * 30);
                            worldPanels[i].Draw(_spriteBatch);
                            number++;
                        }
                    }
                }

                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}