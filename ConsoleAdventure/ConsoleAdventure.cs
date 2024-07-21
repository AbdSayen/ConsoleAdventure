using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.IO;
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
        public static GraphicsDeviceManager _graphics;
        public static SpriteBatch _spriteBatch;
        private static SpriteFont font;

        internal static World world = new World();
        internal static Display display;

        static int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        public static int language = 0;

        public static bool InWorld;
        public static bool isPause;

        public static Vector2 cellSize = new Vector2(18, 19);
        public static Vector2 worldPos = new Vector2(9, 150);

        public static int screenWidth = 1602;
        public static int screenHeight = 912;

        private Color bg = Color.Black;

        public static KeyboardState prekstate;
        public static KeyboardState kstate;

        public static Random rand = new Random();

        private Menu menu;

        internal static bool isExit;

        public static Tags tags = new();

        public static SpriteFont Font => font;

        public static float Width => _graphics.PreferredBackBufferWidth;

        public static float Height => _graphics.PreferredBackBufferHeight;

        public static int FPS => frameRate;

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
            IsFixedTimeStep = true;
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

            menu = new Menu();
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
                    Saves.Save("World");
                    InWorld = false;
                }

                if (!kstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space) && prekstate.IsKeyDown(Microsoft.Xna.Framework.Input.Keys.Space))
                {
                    if (!isPause) 
                        isPause = true;
                    else 
                        isPause = false;
                }
            }       

            else
            {
                menu.MenuUpdate();
                if (isExit) Exit();
            }
            
            base.Update(gameTime);
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
                menu.Draw(_spriteBatch);
            }

            base.Draw(gameTime);
        }
    }
}