using ConsoleAdventure.Settings;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Threading;

namespace ConsoleAdventure
{
    public class ConsoleAdventure : Game
    {
        private GraphicsDeviceManager _graphics;
        private Microsoft.Xna.Framework.Graphics.SpriteBatch _spriteBatch;
        SpriteFont font;

        WorldEngine.World world = new WorldEngine.World();
        string output = "";

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
            Window.AllowUserResizing = true;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            font = Content.Load<SpriteFont>("Fonts/font");
        }

        protected override void Update(GameTime gameTime)
        {
            output = $"{Docs.GetInfo()}\n\n{world.Render()}{Loger.GetLogs()}";
            world.ListenEvents();
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.White);

            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, output, new Vector2(0, 0), Color.Black);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}