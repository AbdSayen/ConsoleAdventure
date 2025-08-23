using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI.System
{
    public class UIButton : UIElement
    {
        private Color currentColor;
        private Color color;
        private Color selectColor;
        private FormatString _currentText;
        private FormatString currentText
        {
            get
            {
                return _currentText;
            }
            set
            {
                _currentText = value;
                Vector2 textVec = ConsoleAdventure.Font.MeasureString(value.String);
                size = textVec.ToPoint();
            }
        }
        private string text;
        private string selectText;

        private Vector2 buttonArrowsOffset = new Vector2(ConsoleAdventure.Font.MeasureString("<").X, 0);

        public UIButton(string text, Color color, Color selectColor, Point screenPosition, string selectText = "", Point size = new(), Point margin = new(), Anchor anchor = Anchor.Center, int zOrder = 0) : base(screenPosition, size, margin, anchor, zOrder)
        {
            this.text = text;
            currentText = new(text);
            this.color = color;
            currentColor = color;
            this.selectColor = selectColor;
            this.selectText = selectText;
        }

        public override bool IsFocusable()
        {
            return true;
        }

        public override void OnFocus()
        {
            hovered = true;
            currentColor = selectColor;
            if (selectText.Length > 0)
            {
                currentText = new(selectText);
            }
        }

        public override void OnDefocus()
        {
            hovered = false;
            currentColor = color;
            currentText = new(text);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            currentText.Draw(spriteBatch, drawPosition, currentColor);
            if (IsHovered()) spriteBatch.DrawString(ConsoleAdventure.Font, "<" + new string(' ', currentText.String.Length) + ">", drawPosition - buttonArrowsOffset, currentColor);
        }
    }
}
