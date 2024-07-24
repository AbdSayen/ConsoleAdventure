using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI
{
    public class ChestUI
    {
        public InfoPanel panel;

        public string items;

        public ChestUI(Point position, string items) 
        {
            this.items = items;
            panel = new InfoPanel(new(10, 40, position.X, position.Y), "Chest", items);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            panel.Draw(spriteBatch);
        }
    }
}
