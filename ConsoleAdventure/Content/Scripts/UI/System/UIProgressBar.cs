using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI.System
{
    public class UIProgressBar : UIBar
    {
        private static string[] loadAnimFrames = { "-", "/", "|", @"\" };

        public string stepText = "load";

        public UIProgressBar(int psize, Point screenPosition, Anchor anchor = Anchor.Center, int zOrder = 0) : base(psize, screenPosition, anchor, zOrder)
        {
            Size = psize;
        }

        int timer;
        public override void Draw(SpriteBatch spriteBatch, Vector2 drawPosition)
        {
            string loadAnimation = loadAnimFrames[(timer / 4) % 4];

            text = $"{stepText} [{GetBarString(' ', '.')}] {Progress}% {loadAnimation}";
            spriteBatch.DrawString(ConsoleAdventure.Font, text, drawPosition, Color.White);
            string progressBarText = $"{new string(' ', stepText.Length)}  {GetBarString('▪', ' ')}";
            spriteBatch.DrawString(ConsoleAdventure.Font, progressBarText, drawPosition, color);

            timer++;
        }
    }
}
