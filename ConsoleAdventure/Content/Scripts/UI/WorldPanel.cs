using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace ConsoleAdventure.Content.Scripts.UI
{
    internal class WorldPanel : BaseUI
    {
        public Color cursorColor = Color.White;

        public int curssor = 0;

        public bool isHover;

        string name = "";

        string seed = "";

        public WorldPanel(Vector2 position, string text, Color color, string name, string seed) : base(position, text, color)
        {
            this.name = name;
            this.seed = seed;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            if(isHover)
            {
                cursorColor = Color.Yellow;
                spriteBatch.DrawString(ConsoleAdventure.Font, "^", Position + (new Vector2(9 * (39 + (curssor * 2)), 19 * 2)), Color.Yellow);
            }

            else
            {
                cursorColor = Color.White;
            }

            spriteBatch.DrawFrame(ConsoleAdventure.Font, Utils.GetPanel(new(46, 4), 0), Position, cursorColor);
            spriteBatch.DrawFrame(ConsoleAdventure.Font, Utils.GetPanel(new(7, 4), 0), Position, cursorColor);
            spriteBatch.DrawString(ConsoleAdventure.Font, " Λ \n╱ ╲", Position + (new Vector2(14, 19) * 1), Color.White);

            spriteBatch.DrawString(ConsoleAdventure.Font, $"Name:{name}", Position + (new Vector2(9 * 8, 19 * 1)), Color.White);
            spriteBatch.DrawString(ConsoleAdventure.Font, $"Seed:{seed}", Position + (new Vector2(9 * 8, 19 * 2)), Color.White);

            spriteBatch.DrawString(ConsoleAdventure.Font, "►", Position + (new Vector2(9 * 39, 19 * 1)), Color.White);
            spriteBatch.DrawString(ConsoleAdventure.Font, "≡", Position + (new Vector2(9 * 41, 19 * 1)), Color.White);
            spriteBatch.DrawString(ConsoleAdventure.Font, "Ս", Position + (new Vector2(9 * 43, 19 * 1)), Color.White);
        }
    }
}
