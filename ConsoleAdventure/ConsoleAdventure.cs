using ConsoleAdventure.Content.Scripts.UI;
using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;

using System.Linq;

namespace ConsoleAdventure
{
    public class ConsoleAdventure : Game
    {
        private GraphicsDeviceManager _graphics;
        public static SpriteBatch _spriteBatch;
        private RenderTarget2D _renderTarget;

        private static SpriteFont font;
        private static SpriteFont fontBig;

        private static Effect mask;

        public static Texture2D[] maskTexture = new Texture2D[1];

        World world = new World();
        Display display;
        bool InWorld = false;

        public static int screenWidth = 1602;
        public static int screenHeight = 912;

        private Color bg = Color.Black; //new Color(25, 25, 25);

        public static KeyboardState prekstate;
        public static KeyboardState kstate;

        private MenuButton[] menuButtons = new MenuButton[3];

        public static SpriteFont Font => font;

        public static SpriteFont FontBig => fontBig;

        public ConsoleAdventure()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            _graphics.PreferredBackBufferHeight = 64;
            Window.Title = $"Console Adventure {Docs.version}. By Bonds";
            _graphics.PreferredBackBufferWidth = screenWidth;
            _graphics.PreferredBackBufferHeight = screenHeight;
            _graphics.ApplyChanges();
            Window.AllowUserResizing = true;

            _renderTarget = new RenderTarget2D(GraphicsDevice, screenWidth, screenHeight, false, SurfaceFormat.Color, DepthFormat.None);

            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/font");
            fontBig = Content.Load<SpriteFont>("Fonts/fontBig");

            mask = Content.Load<Effect>("Effects/Mask");
            maskTexture[0] = Content.Load<Texture2D>("Textures/BigMaskTexture");

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
                int startPos = 250;
                int indent = 100;

                menuButtons[i] = new MenuButton(Vector2.Zero, text, 1.5f, 2, new Color(230, 230, 230), menuButtonTypes[i]);
                menuButtons[i].Center = new Vector2((_graphics.PreferredBackBufferWidth / 2), startPos + (indent * i));
            }

            menuButtons[0].isHover = true;
        }

        protected override void Update(GameTime gameTime)
        {
            prekstate = kstate;
            kstate = Keyboard.GetState();

            if (InWorld)
            {
                world.ListenEvents();
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
                int waitTime = 20;

                if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Down) && timer >= waitTime)
                {
                    if (menuButtons[i].isHover)
                    {
                        menuButtons[i].isHover = false;

                        if(i != menuButtons.Length - 1)
                            menuButtons[i + 1].isHover = true;
                        else
                            menuButtons[0].isHover = true;

                        timer = 0;
                    }
                }

                if (kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Up) && timer >= waitTime)
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
                        InWorld = true;
                        display = new Display(world);
                    }

                    if (menuButtons[i].type == 2)
                    {
                        Exit();
                    }
                }     
            }

            timer++;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(bg);

            if (InWorld)
            {
                _spriteBatch.Begin();

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

                for (int i = 0; i < menuButtons.Length; i++)
                {
                    menuButtons[i].Draw(_spriteBatch);
                }
                
                _spriteBatch.End();
            }

            base.Draw(gameTime);
        }
    }
}