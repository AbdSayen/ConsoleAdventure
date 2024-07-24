using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConsoleAdventure.Content.Scripts.UI
{
    public class InfoPanel : BaseUI
    {
        string name = "";

        Texture2D pixel;

        public InfoPanel(Rectangle rectangle, string name, string text) : base(rectangle, Color.White)
        {
            this.name = name;
            this.text = text;

            pixel = new Texture2D(ConsoleAdventure._graphics.GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.Black });
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteFont font = ConsoleAdventure.Font;

            spriteBatch.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, 64 * 9, 30 * 19), Color.White);
            spriteBatch.DrawFrame(font, Utils.GetPanel(new(64, 30), 0), Position, Color.White);
            spriteBatch.DrawFrame(font, Utils.GetPanel(new(64, 3), 0), Position, Color.White);

            spriteBatch.DrawString(font, name, Position + (new Vector2(9 * 8, 19 * 1)), Color.White);
            spriteBatch.DrawString(font, text, Position + (new Vector2(9 * 3, 19 * 3)), Color.White);
        }
    }
}
