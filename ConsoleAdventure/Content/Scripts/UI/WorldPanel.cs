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

        public WorldPanel(Rectangle rectangle, string name, string seed) : base(rectangle, Color.White)
        {
            this.name = name;
            this.seed = seed;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            SpriteFont font = ConsoleAdventure.Font;

            if(isHover)
            {
                cursorColor = Color.Yellow;
                spriteBatch.DrawString(font, "^", Position + (new Vector2(9 * (39 + (curssor * 2)), 19 * 2)), Color.Yellow);
            }

            else
            {
                cursorColor = Color.White;
            }

            spriteBatch.DrawFrame(font, Utils.GetPanel(new(46, 4), 0), Position, cursorColor);
            spriteBatch.DrawFrame(font, Utils.GetPanel(new(7, 4), 0), Position, cursorColor);
            spriteBatch.DrawString(font, " Λ \n╱ ╲", Position + (new Vector2(14, 19) * 1), Color.White);

            spriteBatch.DrawString(font, TextAssets.Name + name, Position + (new Vector2(9 * 8, 19 * 1)), Color.White);
            spriteBatch.DrawString(font, TextAssets.Seed + seed, Position + (new Vector2(9 * 8, 19 * 2)), Color.White);

            spriteBatch.DrawString(font, "►", Position + (new Vector2(9 * 39, 19 * 1)), Color.White);
            spriteBatch.DrawString(font, "≡", Position + (new Vector2(9 * 41, 19 * 1)), Color.White);
            spriteBatch.DrawString(font, "Ս", Position + (new Vector2(9 * 43, 19 * 1)), Color.White);
        }
    }
}
