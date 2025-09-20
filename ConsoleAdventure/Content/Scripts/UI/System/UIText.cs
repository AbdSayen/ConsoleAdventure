using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;

namespace ConsoleAdventure.Content.Scripts.UI.System
{
    public enum Align
    {
        Left,
        Right,
        Center
    }

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

        private Align align = Align.Left;

        public UIText(string text, Color color, Point screenPosition, Point size = new(), Align align = Align.Left, Anchor anchor = Anchor.Center, int zOrder = 0) : base(screenPosition, size, anchor, zOrder)
        {
            this.text = new FormatString(text);
            this.color = color;
            this.align = align;
        }

        public UIText(FormatString ftext, Color color, Point screenPosition, Point size = new(), Align align = Align.Left, Anchor anchor = Anchor.Center, int zOrder = 0) : base(screenPosition, size, anchor, zOrder)
        {
            text = ftext;
            this.color = color;
            this.align = align;
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            if (align == Align.Left)
            {
                text.Draw(spriteBatch, drawPosition, color);
                return;
            }
            else
            {
                string[] text_source = text.BaseString.Split("\n");
                string[] text_source_format = text.String.Split("\n");
                for (int i = 0; i < text_source.Length; i++)
                {
                    Vector2 msr = ConsoleAdventure.Font.MeasureString(text_source_format[i]);
                    Vector2 offset = Vector2.Zero;
                    if (align == Align.Right) offset = new Vector2(size.X - msr.X, i * msr.Y);
                    else if (align == Align.Center) offset = new Vector2((size.X - msr.X) / 2, i * msr.Y);
                    new FormatString(text_source[i]).Draw(spriteBatch, drawPosition + offset, color);
                }
            }
        }
    }
}
