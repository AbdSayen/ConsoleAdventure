using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace ConsoleAdventure.Content.Scripts.UI.System
{
    public class UIWorldPanel : UIElement
    {
        private FormatString worldName;
        private FormatString seedText;

        private Color cursorColor = Color.White;

        public UIWorldPanel(string wname, string seed, Point screenPosition, string name = null, Point margin = new(), Anchor anchor = Anchor.Center, int zOrder = 0) : base(screenPosition, new(46*9, 4*19), margin, anchor, zOrder, name: name)
        {
            if (name == null) this.name = wname;
            worldName = new FormatString(TextAssets.Name + wname);
            seedText = new FormatString(TextAssets.Seed + seed);
        }

        public override void OnConfirmKeyUp()
        {

        }

        public override void OnConfirmKeyDown()
        {

        }

        public override bool IsFocusable()
        {
            return true;
        }

        public override void OnFocus()
        {
            hovered = true;
            cursorColor = Color.Yellow;
        }

        public override void OnDefocus()
        {
            hovered = false;
            cursorColor = Color.White;
        }

        public override void Update()
        {
            
        }

        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            spriteBatch.DrawFrame(ConsoleAdventure.Font, Utils.GetPanel(new(46, 4), 0), drawPosition, cursorColor);
            spriteBatch.DrawFrame(ConsoleAdventure.Font, Utils.GetPanel(new(7, 4), 0), drawPosition, cursorColor);
            spriteBatch.DrawString(ConsoleAdventure.Font, " Λ \n╱ ╲", drawPosition + new Vector2(14, 19), Color.White);

            worldName.Draw(spriteBatch, drawPosition + new Vector2(72, 19));
            seedText.Draw(spriteBatch, drawPosition + new Vector2(72, 38));

            spriteBatch.DrawString(ConsoleAdventure.Font, "► ≡ Ս", drawPosition + new Vector2(351, 19), Color.White);
        }
    }
}
