using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI.System
{
    public class UIBar : UIElement
    {
        internal int Size;

        public int _progress;
        public int Progress
        {
            get
            {
                return _progress;
            }
            set
            {
                _progress = Math.Clamp(value, 0, 100);
            }
        }
        public Color color = Color.LightGreen;

        private string _text;
        internal string text
        {
            get
            {
                return _text;
            }
            set
            {
                _text = value;
                Vector2 textVec = ConsoleAdventure.Font.MeasureString(value);
                size = textVec.ToPoint();
            }
        }

        public UIBar(int psize, Point screenPosition, Anchor anchor = Anchor.Center, int zOrder = 0) : base(screenPosition, new(), anchor, zOrder)
        {
        }

        internal string GetBarString(char fillSymbol, char emptySymbol)
        {
            int num = (int)((Progress / 100f) * Size);
            return $"{new string(fillSymbol, num)}{new string(emptySymbol, Size - num)}";
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            text = $"[{GetBarString(' ', '.')}]";
            spriteBatch.DrawString(ConsoleAdventure.Font, text, drawPosition, Color.White);
            string progressBarText = $" {GetBarString('▪', ' ')}";
            spriteBatch.DrawString(ConsoleAdventure.Font, progressBarText, drawPosition, color);
        }
    }
}
