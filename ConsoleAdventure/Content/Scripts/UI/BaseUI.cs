using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConsoleAdventure.Content.Scripts.UI
{
    public class BaseUI
    {
        private Vector2 position;

        public string text = "";

        public Rectangle rectangle = new();

        public Vector2 size;

        public float rotation;

        public Color color;

        public byte fontSize;

        public Vector2 Position
        {
            get => position;
            set
            {
                position = value;
                //Vector2 textSize = ConsoleAdventure.Font.MeasureString(text);
                //Vector2 scale = new(size.X + (fontSize == 2 ? 2 : 1), size.Y + (fontSize == 2 ? 2 : 1));
                //rectangle = new Rectangle(position.ToPoint(), (textSize * scale).ToPoint());
                SpriteFont font = fontSize == 2 ? ConsoleAdventure.FontBig : ConsoleAdventure.Font;
                Vector2 textSize = font.MeasureString(text);
                rectangle = new Rectangle(position.ToPoint(), (textSize * size).ToPoint());
            }
        }

        public Vector2 Center
        {
            get => rectangle.Center.ToVector2();
            set
            {
                Vector2 halfSize = new Vector2(rectangle.Width / 2, rectangle.Height / 2);
                rectangle.Location = (value - halfSize).ToPoint();
                position = rectangle.Location.ToVector2();
            }
        }

        public BaseUI(Vector2 position, string text, Vector2 size, byte fontSize, Color color)
        {      
            this.text = text;
            this.size = size;
            this.color = color;
            this.fontSize = fontSize;
            Position = position;
        }

        public BaseUI(Vector2 position, string text, float size, byte fontSize, Color color)
        {
            this.text = text;
            this.size = new(size, size);
            this.color = color;
            this.fontSize = fontSize;
            Position = position;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            SpriteFont font = fontSize == 2 ? ConsoleAdventure.FontBig : ConsoleAdventure.Font;

            spriteBatch.DrawString(font, text, position, color, rotation, Vector2.Zero, size, 0, 0);
        }
    }
}
