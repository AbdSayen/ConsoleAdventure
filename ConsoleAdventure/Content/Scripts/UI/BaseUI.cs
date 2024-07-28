using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConsoleAdventure.Content.Scripts.UI
{
    public class BaseUI
    {
        private Vector2 position;

        public string text = "";

        public Rectangle rectangle = new();

        public Color color;

        public bool updateToText = true;

        public bool isHover = false;

        public Vector2 Position
        {
            get => position;
            set
            {
                position = value;
                UpdateRectToTextSize();
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

        public BaseUI(Vector2 center, string text, Color color)
        {
            this.text = text;
            this.color = color;
            Position = Vector2.Zero;
            Center = center;
        }

        public BaseUI(string text, Vector2 position, Color color)
        {
            this.text = text;
            this.color = color;
            Position = position;
        }

        public BaseUI(Vector2 center, Color color, string localization)
        {
            text = Localization.GetTranslation("UI", localization);
            this.color = color;
            Position = Vector2.Zero;
            Center = center;
        }

        public BaseUI(Rectangle rectangle, Color color)
        {   
            Position = Vector2.Zero;
            this.rectangle = rectangle;
            Center = rectangle.Location.ToVector2();
            this.color = color;
        }

        public BaseUI(Rectangle rectangle, Color color, bool updateToText)
        {
            this.updateToText = updateToText;
            Position = Vector2.Zero;
            this.rectangle = rectangle;
            Center = rectangle.Location.ToVector2();
            this.color = color;
        }

        public void UpdateRectToTextSize()
        {
            if (!updateToText) return;
            Vector2 textSize = ConsoleAdventure.Font.MeasureString(text);
            rectangle = new Rectangle(position.ToPoint(), textSize.ToPoint());
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(ConsoleAdventure.Font, text, position, color, 0, Vector2.Zero, 1f, 0, 0);
        }
    }
}
