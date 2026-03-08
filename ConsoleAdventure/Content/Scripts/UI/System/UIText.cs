using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Text;
using System.Windows.Forms;

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
        private bool SizeByContent = true;

        private FormatString _text;

        public FormatString text {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                if (SizeByContent)
                {
                    Vector2 textVec = ConsoleAdventure.Font.MeasureString(value.String);
                    size = textVec.ToPoint();
                }

                text_source = value.BaseString.Split("\n");
                text_source_format = value.String.Split("\n");
            }
        }
        public Color color;

        private Align align = Align.Left;

        private string[] text_source;
        private string[] text_source_format;

        public UIText(string text, Color color, Point screenPosition, Point size = new(), Align align = Align.Left, Anchor anchor = Anchor.Center, bool sizeByContent = true, int zOrder = 0) : base(screenPosition, size, anchor, zOrder)
        {
            SizeByContent = sizeByContent;
            this.text = new FormatString(text);
            this.color = color;
            this.align = align;
        }

        public UIText(FormatString ftext, Color color, Point screenPosition, Point size = new(), Align align = Align.Left, Anchor anchor = Anchor.Center, bool sizeByContent = true, int zOrder = 0) : base(screenPosition, size, anchor, zOrder)
        {
            SizeByContent = sizeByContent;
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
