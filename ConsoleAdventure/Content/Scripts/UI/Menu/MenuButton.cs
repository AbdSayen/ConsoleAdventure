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
        public Color cursorColor = Color.Yellow;

        public byte type;

        public MenuButton(Vector2 position, string kay, Color color, byte type) : base(position, color, kay)
        {
            this.type = type;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if (isHover)
            {
                spriteBatch.DrawString(ConsoleAdventure.Font, "<", Position - new Vector2(10, 0), cursorColor, 0, Vector2.Zero, 1, 0, 0);
                spriteBatch.DrawString(ConsoleAdventure.Font, ">", Position + new Vector2(rectangle.Width + 2, 0), cursorColor, 0, Vector2.Zero, 1, 0, 0);
            }       
        }
    }
}
