using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace ConsoleAdventure.Content.Scripts.UI.System
{
    public class UIText : UIElement
    {
        private FormatString _text;

        internal FormatString text {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                Vector2 textVec = ConsoleAdventure.Font.MeasureString(value.String);
                size = textVec.ToPoint();
            }
        }
        internal Color color;
        public UIText(string text, Color color, Point screenPosition, Point size = new(), Anchor anchor = Anchor.Center, int zOrder = 0) : base(screenPosition, size, anchor, zOrder)
        {
            this.text = new FormatString(text);
            this.color = color;
        }

        public UIText(FormatString ftext, Color color, Point screenPosition, Point size = new(), Anchor anchor = Anchor.Center, int zOrder = 0) : base(screenPosition, size, anchor, zOrder)
        {
            text = ftext;
            this.color = color;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            text.Draw(spriteBatch, drawPosition, color);
        }
    }
}
