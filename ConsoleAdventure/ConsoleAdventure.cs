using ConsoleAdventure.Content.Scripts.InputLogic;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Content.Scripts.UI;
using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.IO;


namespace ConsoleAdventure
{
    public class ConsoleAdventure : Game
    {
        public static GraphicsDeviceManager _graphics;
        public static SpriteBatch _spriteBatch;
        private static SpriteFont font;

        internal static World world;
        internal static Display display;

        static int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        //public static int language = 0;

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
            if (File.Exists(Program.savePath + "settings.json")) // Если файл существует
                SettingsSystem.LoadSettings(); // Загружаем сохраненные настройки

            // Инициализируем тут все настройки
            // (Не нужно бояться что это перезапишет сохраненные данные,
            // инициализация только создаст значения которые не определены,
            // это может случится в следующих случаях: Первый запуск приложения или
            // Вышло обновление приложения где добавлена новая настройка)
            SettingsSystem.InitSetting("Options", "Language");
            //                          ^^^^         ^^^^
            //                     Тип настроек     Ключ настройки

            //  Тут такая же система как в локализации
            
            
            _graphics = new GraphicsDeviceManager(this);
            Localization.Load();
        }

        public static void CreateWorld(string name, int seed, bool isfullGenerate = true)
        {
            world = new World(name, seed, isfullGenerate);
            display = new Display(world);
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
            
            CaModLoader.InitializeMods();
            menu = new Menu();
            CaModLoader.RunMods();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/font");

            CaModLoader.PreLoadMods();
            CaModLoader.LoadMods();
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
                    Saves.Save(world.name);
                    InWorld = false;
                }

                if (!kstate.IsKeyDown(InputConfig.Pause) && prekstate.IsKeyDown(InputConfig.Pause))
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
                if (isExit)
                {
                    SettingsSystem.SaveSettings(); // Перед выходом нужно сохранить значения настроек
                    Exit();
                }
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