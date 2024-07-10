using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI
{
    internal class MenuButton : BaseUI
    {
        private Color cursorColor = Color.Yellow;

        public bool isHover;

        public byte type;

        public MenuButton(Vector2 position, string text, float size, byte fontSize, Color color, byte type) : base(position, text, size, fontSize, color)
        {
            this.type = type;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (isHover)
            {
                spriteBatch.DrawString(ConsoleAdventure.Font, "<", Position - new Vector2(10 * size.X, 0), cursorColor, rotation, Vector2.Zero, size, 0, 0);
                spriteBatch.DrawString(ConsoleAdventure.Font, ">", Position + new Vector2(rectangle.Width + 2, 0), cursorColor, rotation, Vector2.Zero, size, 0, 0);
            }       
        }
    }
}
