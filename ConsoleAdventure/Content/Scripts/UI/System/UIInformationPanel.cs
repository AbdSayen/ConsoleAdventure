using ConsoleAdventure.Content.Scripts.InputLogic;
using ConsoleAdventure.Content.Scripts.UI.System.Containers;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;

namespace ConsoleAdventure.Content.Scripts.UI.System
{
    public class UIInformationPanel : UIElement
    {
        private FormatString name;
        private FormatString text;
        private Color nameColor;
        private Color textColor;
        private UIText enterhint;
        private UIContainer returnControlTo;

        public UIInformationPanel(Point pos, Point size, string name, string text, Color? nameColor = null, Color? textColor = null, UIContainer returnControlTo = null) : base(pos, size, Anchor.Center)
        {
            this.name = new FormatString(name);
            this.text = new FormatString(text);
            this.nameColor = nameColor ?? Color.White;
            this.textColor = textColor ?? Color.White;
            this.returnControlTo = returnControlTo;

            enterhint = new UIText("> Press Enter to close <", Color.Yellow, new(), new Point(size.X, 0), Align.Center, Anchor.Center, false);
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            Utils.DrawAnySizeFrame(spriteBatch, drawPosition.ToPoint(), size, 2, Color.White);
            spriteBatch.Draw(ConsoleAdventure.pixel, new Rectangle(drawPosition.ToPoint() + new Point(0, 19 * 3), new Point(size.X, 2)), Color.White);

            name.Draw(spriteBatch, drawPosition + new Vector2(9 * 8, 19 * 1), nameColor);
            text.Draw(spriteBatch, drawPosition + new Vector2(9 * 3, 19 * 4), textColor);

            //spriteBatch.DrawString(ConsoleAdventure.Font, , drawPosition + new Vector2(size.X/2, size.Y - 19), Utils.TimeGradient(new List<Color>() { Color.Yellow, Color.Black }));
            enterhint.Draw(spriteBatch, drawPosition + new Vector2(0, size.Y - 19));
        }

        public override void Update()
        {
            if (!IsHovered()) { return; }
            if (Input.PostClick(InputConfig.NavigationSelect))
            {
                if (returnControlTo != null) FocusManager.SetFocus(returnControlTo);
                else FocusManager.SetFocus(GetParent());
                Destroy();
            }
        }
    }
}
