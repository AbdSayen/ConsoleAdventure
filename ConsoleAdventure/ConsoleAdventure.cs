using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConsoleAdventure
{
    public class ConsoleAdventure : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        SpriteFont font;

        World world = new World();
        Display display;

        public ConsoleAdventure()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            display = new Display(world);

            _graphics.PreferredBackBufferHeight = 64;
            Window.Title = $"Console Adventure {Docs.version}. By Bonds";
            _graphics.PreferredBackBufferWidth = 1600;
            _graphics.PreferredBackBufferHeight = 900;
            _graphics.ApplyChanges();
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
            world.ListenEvents();
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(new Color(25, 25, 25));

            _spriteBatch.Begin();
            _spriteBatch.DrawString(font, display.DisplayInfo(), new Vector2(10, 10), Color.Gray);
            _spriteBatch.DrawString(font, display.DisplayFloorLayer(), new Vector2(10, 150), Color.Gray);
            _spriteBatch.DrawString(font, display.DisplayBlocksLayer(), new Vector2(10, 150), Color.White);
            _spriteBatch.DrawString(font, display.DisplayItemsLayer(), new Vector2(10, 150), Color.Yellow);
            _spriteBatch.DrawString(font, display.DisplayMobsLayer(), new Vector2(10, 150), Color.Yellow);
            _spriteBatch.DrawString(font, display.DisplayInventory(), new Vector2(_graphics.PreferredBackBufferWidth - 300, 10), Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}