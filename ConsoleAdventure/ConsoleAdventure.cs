using CaModLoaderAPI;
using ConsoleAdventure.Content.Scripts.Audio;
using ConsoleAdventure.Content.Scripts.Debug.Commands;
using ConsoleAdventure.Content.Scripts.InputLogic;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Content.Scripts.Settings;
using ConsoleAdventure.Content.Scripts.UI;
using ConsoleAdventure.Content.Scripts.WorldEngine.Events;
using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using SharpDX.Direct3D9;
using SharpDX.MediaFoundation;
using System;
using System.Collections.Generic;
using System.Drawing.Configuration;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleAdventure
{
    public class ConsoleAdventure : Game
    {
        public static GraphicsDeviceManager _graphics;
        public static SpriteBatch _spriteBatch;
        private static SpriteFont font;

        public static World world;
        public static Display display;

        public static List<Recipe> recipes = new List<Recipe>();
        public static List<Recipe> availableRecipes = new List<Recipe>();

        static int frameRate = 0;
        int frameCounter = 0;
        TimeSpan elapsedTime = TimeSpan.Zero;

        public static bool InWorld;
        public static bool isPause;

        public static Vector2 cellSize = new Vector2(18, 19);
        public static Vector2 worldPos = new Vector2(9, 150);
        public static Position startDisplay;
        public static Position endDisplay;

        public static int screenWidth = 1602;
        public static int screenHeight = 912;

        private Color bg = Color.Black;

        public static KeyboardState prekstate;
        public static KeyboardState kstate;

        public static MouseState mouse = Mouse.GetState();
        public static MouseState oldMouse = Mouse.GetState();
        public static Vector2 mousePosition;

        public static Random rand = new Random();

        public static ProgressBar progressBar;

        public static Menu menu;

        internal static bool isExit;

        public static int curDeep = 1;
        public static int StartDeep = 1;

        public static Tags tags = new();

        public static ExceptionLogger logger;

        public static Texture2D pixel;

        public static bool BlockHotKey { get; set; } = false;

        public static SpriteFont Font => font;

        public static float Width => _graphics.PreferredBackBufferWidth;

        public static float Height => _graphics.PreferredBackBufferHeight;

        public static int FPS => frameRate;

        public static MenuState MenuState => menu.State;

        public static Position MouseWorld
        {
            get
            {
                Vector2 offset = (worldPos + ((new Vector2(30, 15) - world.GetLocalPlayer().position.ToVector2()) * cellSize));
                Vector2 fieldPos = ((mousePosition - offset) / cellSize).ToPoint().ToVector2();
                return fieldPos.ToPosition();
            }
        }

        private bool _isFirstUpdate = true;

        public static bool WindowActive { get; private set; }

        public ConsoleAdventure()
        {
            logger = new ExceptionLogger("gameLog.txt");
            //Task task = new Task(UpdateLogs);
            //task.Start();

            logger.AddMessage("Starting...");
            logger.AddMessage("Settings loading ...");
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

            //  Тут такая же система как в локализации.

            InputConfig.Init();
            InputConfig.Load();

            if (File.Exists(Program.pcIdPath)) // create pc id
                NetworkManager.pcId = File.ReadAllText(Program.pcIdPath);
            else
            {
                FileStream f = File.Create(Program.pcIdPath);
                NetworkManager.pcId = Guid.NewGuid().ToString();
                f.Write(Encoding.UTF8.GetBytes(NetworkManager.pcId));
                f.Close();
            }

            _graphics = new GraphicsDeviceManager(this);

            Localization.Load();
        }

        /*int LogIterationCount;
        private static StringWriter stringWriter = new StringWriter();
        private static readonly object lockObj = new object();

        private void UpdateLogs()
        {
            while (true)
            {
                //if(LogIterationCount % 4 == 0)
                lock (lockObj)
                {
                    Console.SetOut(stringWriter);
                    logger.AddLog(stringWriter.ToString());
                    stringWriter.GetStringBuilder().Clear();
                }
                //LogIterationCount++;
            } 
        }*/

        public static async Task CreateWorld(string name, int seed, bool isfullGenerate = true, bool inMultiplayer = false)
        {
            world = new World(name, seed);
            world.inMultiplayer = inMultiplayer;
   
            await world.Initialize(isfullGenerate);
            display = new Display(world);
        }

        protected override void Initialize()
        {
            logger.AddMessage("Initializing...");
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

            pixel = new Texture2D(_graphics.GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.Black });

            CaModLoader.InitializeMods();
            menu = new Menu();

            SoundEngine.Init(44100, 44100, Microsoft.Xna.Framework.Audio.AudioChannels.Stereo);
            Command.InitCommands();

            world = new World("empty", 0);
            Main.InitTransformsTypes(Main.vanillaTypesInitialized);
            WorldIO.InitContent();

            CaModLoader.RunMods();
        }

        protected override void LoadContent()
        {
            logger.AddMessage("Content loading...");
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/font");

            string musicDir = "AudioFiles/Music/";
            MusicEngine.AddSong("ConsoleAdventure.StrangeWorld", Content.Load<Song>(musicDir + "StrangeWorld"));
            MusicEngine.AddSong("ConsoleAdventure.ItsMagicRain", Content.Load<Song>(musicDir + "ItsMagicRain"));
            MusicEngine.ChangeSong("ConsoleAdventure.StrangeWorld");

            CaModLoader.PreLoadMods();
            CaModLoader.LoadMods();
            logger.AddMessage("Done!");

            StringBuilder sb = new StringBuilder();
            Texture2D logo = Content.Load<Texture2D>("logoAnim");

            Color[] colors = new Color[logo.Width * logo.Height];
            logo.GetData(colors);


            for (int y = 0; y < logo.Height; y++)
            {
                for (int x = 0; x < logo.Width; x++)
                {
                    Color color = colors[x + y * logo.Width];

                    if      (Equals(color, new(255, 255, 255))) sb.Append("##");
                    else if (Equals(color, new(167, 167, 167))) sb.Append("≈≈");
                    else if (Equals(color, new(96, 96, 96)))    sb.Append("::");
                    else if (Equals(color, new(33, 33, 33)))    sb.Append("..");
                    else if (Equals(color, new(255, 250, 199))) sb.Append("♦♦");
                    else if (Equals(color, new(255, 233, 0)))   sb.Append("✶ ");
                    else if (Equals(color, new(255, 156, 0)))   sb.Append("☼ ");
                    else if (Equals(color, new(255, 104, 0)))   sb.Append("◌ ");
                    else                                        sb.Append("  ");
                }

                sb.Append("\r\n");
            }

            TextAssets.StartLogo = sb.ToString();

            bool Equals(Color c1, Color c2)
            {
                return c1.R == c2.R && c1.G == c2.G && c1.B == c2.B;
            }
        }

        protected override void Update(GameTime gameTime)
        {
            WindowActive = IsActive;

            prekstate = kstate;
            kstate = Keyboard.GetState();

            MouseState currentMouseState = Mouse.GetState(Window);
            if (currentMouseState.X != mouse.X || currentMouseState.Y != mouse.Y)
                mousePosition = new Vector2(currentMouseState.X, currentMouseState.Y);

            oldMouse = mouse;
            mouse = currentMouseState;

            elapsedTime += gameTime.ElapsedGameTime;
            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime = TimeSpan.Zero;
                frameRate = frameCounter;
                frameCounter = 0;
            }
            //frameCounter++;

            MusicEngine.Update();

            if (InWorld)
            {
                if (_isFirstUpdate)
                {
                    world.Start?.Invoke();
                    _isFirstUpdate = false;
                }

                world.ListenEvents();

                if (kstate.IsKeyDown(InputConfig.WorldExit.key) && !world.isCmdOpen)
                {
                    NetworkManager.DisconectClient();
                    if (NetworkManager.Id == 0 || NetworkManager.Id == -1) WorldIO.Save(world.name);
                    InWorld = false;
                    menu.CloseAllPages();
                    Loger.ClearLogs();
                    world = new("empty", 0);
                }

                if (!kstate.IsKeyDown(InputConfig.Pause.key) && prekstate.IsKeyDown(InputConfig.Pause.key) && !ConsoleAdventure.BlockHotKey)
                {
                    isPause = !isPause;
                }

                MusicEngine.ChangeSong("ConsoleAdventure.StrangeWorld", -10);


                int l = -1;
                if (Input.PostClick(Keys.F11)) l = 0;
                if (Input.PostClick(Keys.F12)) l = 1;

                if (l > -1)
                {
                    try 
                    {
                        System.Drawing.Bitmap bitmap = display.MapScreen(l);
                        if (!Directory.Exists("Screens")) Directory.CreateDirectory("Screens");
                        int count = Directory.GetFiles("Screens", "*.png").Length;

                        bitmap.Save($"Screens/map{count}.png");
                    }

                    catch 
                    { 
                    }
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
        int animTimer;
        int frame;
        protected override void Draw(GameTime gameTime)
        {
            
            GraphicsDevice.Clear(bg);
            CaModLoader.PreDrawMods(_spriteBatch, gameTime);

            if (InWorld)
            {
                _spriteBatch.Begin();

                _spriteBatch.DrawString(font, display.DisplayInfo(), new Vector2(10, 10), Color.Gray);
                _spriteBatch.DrawString(font, display.TransformTooltip(), new Vector2(197, 10), Color.Gray);

                if (CaModLoader.PreDrawWorldMods(_spriteBatch, gameTime, world))
                    display.DrawWorld();

                display.DisplayInventory(new Vector2(_graphics.PreferredBackBufferWidth - 240, 10));

                _spriteBatch.DrawString(font, $"FPS: {(int)frameRate}", new Vector2(10, _graphics.PreferredBackBufferHeight - 30), Color.White);

                CaModLoader.PostDrawWorldMods(_spriteBatch, gameTime, world);

                _spriteBatch.End();
            }

            else
            {
                if (kstate.IsKeyDown(Keys.C) && kstate.IsKeyDown(Keys.A) && kstate.IsKeyDown(Keys.Space) && menu.State == MenuState.mainScreen)
                {
                    _spriteBatch.Begin();

                    int length = (64 * 32) + 64;
                    StringBuilder sb = new();
                    int start = frame + length;
                    for (int i = 0; i < length; i++)
                    {
                        sb.Append(TextAssets.StartLogo[i + start]);
                    }

                    _spriteBatch.DrawString(font, sb.ToString(), new((Width / 2) - (32 * 9), (Height / 2) - (16 * 19)), Color.White);

                    if (animTimer % 7 == 0)
                    {
                        frame += length;
                    }

                    if (frame + length >= TextAssets.StartLogo.Length) frame = 0;

                    _spriteBatch.End();

                    animTimer++;
                }

                else
                {
                    menu.Draw(_spriteBatch);
                    frame = 0;
                }
            }

            CaModLoader.PostDrawMods(_spriteBatch, gameTime);

            frameCounter++;
            base.Draw(gameTime);
        }
    }
}
