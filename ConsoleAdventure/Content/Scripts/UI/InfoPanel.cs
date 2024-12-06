using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConsoleAdventure.Content.Scripts.UI
{
    public class InfoPanel : BaseUI
    {
        string name = "";
        Texture2D pixel;
        public bool isVisible { get; set; }

        private Color nameColor;
        private Color textColor;

        public InfoPanel(Rectangle rectangle, string name, string text, Color? nameColor = null, Color? textColor = null) : base(rectangle, Color.White)
        {
            this.name = name;
            this.text = text;
            this.isVisible = true;

            pixel = new Texture2D(ConsoleAdventure._graphics.GraphicsDevice, 1, 1);
            pixel.SetData(new Color[] { Color.Black });

            if (nameColor == null) nameColor = Color.White;
            if (textColor == null) textColor = Color.White;

            this.nameColor = (Color)nameColor;
            this.textColor = (Color)textColor;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            if (!isVisible) return;

            SpriteFont font = ConsoleAdventure.Font;

            spriteBatch.Draw(pixel, new Rectangle((int)Position.X, (int)Position.Y, rectangle.Width * 9, rectangle.Height * 19), Color.White);
            spriteBatch.DrawFrame(font, Utils.GetPanel(new(rectangle.Width, rectangle.Height), 0), Position, Color.White);
            spriteBatch.DrawFrame(font, Utils.GetPanel(new(rectangle.Width, 3), 0), Position, Color.White);

            spriteBatch.DrawString(font, name, Position + (new Vector2(9 * 8, 19 * 1)), nameColor);
            spriteBatch.DrawString(font, text, Position + (new Vector2(9 * 3, 19 * 3)), textColor);
        }
    }
}
