using CaModLoaderAPI;
using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.Audio;
using ConsoleAdventure.Content.Scripts.Debug.Commands;
using ConsoleAdventure.Content.Scripts.InputLogic;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Content.Scripts.Settings;
using ConsoleAdventure.Content.Scripts.UI;
using ConsoleAdventure.Content.Scripts.UI.System;
using ConsoleAdventure.Content.Scripts.UI.System.Containers;
using ConsoleAdventure.Content.Scripts.UI.System.Menus;
using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System.Text;
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

        public static UIProgressBar progressBar;

        public static Menu menu;

        internal static bool isExit;

        //public static int StartDeep = 1;

        public static Tags tags = new();

        public static ExceptionLogger logger;

        public static Texture2D pixel;

        public static bool GodMode { get; internal set; }

        public static bool ShowTypes { get; internal set; }

        public static bool NoCollision { get; internal set; }

        public static bool BlockHotKey { get; set; } = false;

        public static SpriteFont Font => font;

        public static float Width => _graphics.PreferredBackBufferWidth;

        public static float Height => _graphics.PreferredBackBufferHeight;

        public static int FPS => frameRate;

        public static byte ChunkLoadRadius { get; set; } = 8;

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

        public static int Timer { get; private set; }

        public static UIGroup mainUIgroup;

        public ConsoleAdventure()
        {
            logger = new ExceptionLogger("gameLog.txt");
            //Task task = new Task(UpdateLogs);
            //task.Start();

            logger.AddMessage("Starting...");
            logger.AddMessage("Settings loading ...");

            if (Directory.Exists(Program.savePath + "Worlds"))
                Directory.CreateDirectory(Program.savePath + "Worlds");

            if (File.Exists(Program.savePath + "settings.json")) // Если файл существует
                SettingsSystem.LoadSettings(); // Загружаем сохраненные настройки

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

        public static string[] GetRandomPrettyItem()
        {
            // Получаем текущую сборку
            Assembly currentAssembly = Assembly.GetExecutingAssembly();

            // Получаем все типы в этой сборке
            // Можно также указать другую сборку, например, Assembly.Load("MyAssembly")
            Type[] allTypes = currentAssembly.GetTypes();

            // Фильтруем типы, чтобы найти наследников
            List<Type> allItems = new List<Type>();
            foreach (Type type in allTypes)
            {
                if (type.IsSubclassOf(typeof(Item)))
                {
                    allItems.Add(type);
                }
            }
            Type randomItemType = null;
            Item randomItemInstance = null;
            while (randomItemInstance?.GetTexture().strings.Count < 2 || randomItemInstance == null || randomItemType == null)
            {
                randomItemType = allItems[new Random().Next(allItems.Count)];
                if (!randomItemType.IsAbstract)
                    randomItemInstance = (Item)Activator.CreateInstance(randomItemType);
            }

            return new string[] { randomItemType?.FullName, randomItemInstance?.name };
        }

        public static async Task CreateWorld(string name, int seed, int size, bool isFullGenerate = true, bool inMultiplayer = false)
        {
            world = new World(name, seed, size, true, isFullGenerate);
            world.inMultiplayer = inMultiplayer;

            await world.Initialize(isFullGenerate);
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

            CaModLoader.UpdateModSourcesDLL();

            CaModLoader.InitializeMods();

            CustomColorLoader.Init();
            MusicEngine.Setup();
            menu = new Menu();

            SoundEngine.Init(44100, 44100, Microsoft.Xna.Framework.Audio.AudioChannels.Stereo);
            Command.InitCommands();

            world = new World("empty", 0, 16, false, false);
            Main.InitTransformsTypes(Main.vanillaTypesInitialized);
            WorldIO.InitContent();

            CaModLoader.RunMods();
        }

        UIGroup subgroup;
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

                    if (Equals(color, new(255, 255, 255))) sb.Append("##");
                    else if (Equals(color, new(167, 167, 167))) sb.Append("≈≈");
                    else if (Equals(color, new(96, 96, 96))) sb.Append("::");
                    else if (Equals(color, new(33, 33, 33))) sb.Append("..");
                    else if (Equals(color, new(255, 250, 199))) sb.Append("♦♦");
                    else if (Equals(color, new(255, 233, 0))) sb.Append("✶ ");
                    else if (Equals(color, new(255, 156, 0))) sb.Append("☼ ");
                    else if (Equals(color, new(255, 104, 0))) sb.Append("◌ ");
                    else sb.Append("  ");
                }

                sb.Append("\r\n");
            }

            TextAssets.StartLogo = sb.ToString();

            bool Equals(Color c1, Color c2)
            {
                return c1.R == c2.R && c1.G == c2.G && c1.B == c2.B;
            }

            mainUIgroup = new UIGroup(new(0, 0));
            mainUIgroup.AddElement(new MainMenu());
        }

        protected override void UnloadContent()
        {
            SettingsSystem.SaveSettings(); // Перед выходом нужно сохранить значения настроек
        }

        protected override void Update(GameTime gameTime)
        {
            //FontEditor.ModifyChars(Content);
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
                    if (world.inMultiplayer) NetworkManager.DisconectClient();
                    if (NetworkManager.Id == 0 || NetworkManager.Id == -1) WorldIO.Save(world.name);
                    InWorld = false;
                    menu.CloseAllPages();
                    Loger.ClearLogs();
                    world = new World("empty", 0, 16, false, false);
                }

                if (!kstate.IsKeyDown(InputConfig.Pause.key) && prekstate.IsKeyDown(InputConfig.Pause.key) && !ConsoleAdventure.BlockHotKey)
                {
                    isPause = !isPause;
                }

                MusicEngine.ChangeSong("ConsoleAdventure.StrangeWorld", -10);
            }

            else
            {
                //menu.MenuUpdate();
                mainUIgroup.Update();
                if (isExit)
                {
                    Exit();
                }
            }

            Timer++;

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

                DrawTooltip(_spriteBatch); 

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
                    if (Input.IsKeyDown(Keys.Z) && subgroup.IsVisible())
                    {
                        subgroup.Hide();
                    }

                    _spriteBatch.Begin();
                    mainUIgroup.Draw(_spriteBatch, Vector2.Zero);
                    _spriteBatch.End();
                    //menu.Draw(_spriteBatch);
                    frame = 0;
                }
            }

            CaModLoader.PostDrawMods(_spriteBatch, gameTime);

            frameCounter++;
            base.Draw(gameTime);
        }

        public static void SetFont(SpriteFont newFont)
        {
            font = newFont;
        }

        string oldTooltip = "";
        FormatString formatTooltip;

        private void DrawTooltip(SpriteBatch spriteBatch)
        {
            string baseTooltip = display.TransformTooltip();

            if (baseTooltip != oldTooltip) {
                formatTooltip = new FormatString(baseTooltip, Color.Gray);      
                oldTooltip = baseTooltip;
            } 
            
            formatTooltip.Draw(spriteBatch, new Vector2(197, 10));
        }

        private static void SaveFontTexture(string path)
        {
            Texture2D originalTexture = Font.Texture;

            if (originalTexture.Format != SurfaceFormat.Color)
            {
                // Создаем RenderTarget2D для преобразования формата
                RenderTarget2D renderTarget = new RenderTarget2D(_graphics.GraphicsDevice, originalTexture.Width, originalTexture.Height, false, SurfaceFormat.Color, DepthFormat.None);


                _graphics.GraphicsDevice.SetRenderTarget(renderTarget);
                _graphics.GraphicsDevice.Clear(Color.Transparent);

                using (SpriteBatch spriteBatch = new SpriteBatch(_graphics.GraphicsDevice))
                {
                    spriteBatch.Begin();
                    spriteBatch.Draw(originalTexture, Vector2.Zero, Color.White);
                    spriteBatch.End();
                }

                _graphics.GraphicsDevice.SetRenderTarget(null);

                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    renderTarget.SaveAsPng(stream, renderTarget.Width, renderTarget.Height);
                }

                renderTarget.Dispose();
            }
            else
            {
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    originalTexture.SaveAsPng(stream, originalTexture.Width, originalTexture.Height);
                }
            }
        }
    }
}