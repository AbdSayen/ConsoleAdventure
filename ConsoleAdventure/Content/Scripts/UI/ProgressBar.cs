using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConsoleAdventure.Content.Scripts.UI
{
    public class ProgressBar : Bar
    {
        private static string[] loadAnimFrames = {"-", "/", "|", @"\" };

        public const bool PercentLeft = true;
        public const bool PercentRight = false;

        bool percentPos;

        public string stepText = "load";

        public ProgressBar(Rectangle rectangle, Color color, uint size, bool percentPos) : base(rectangle, color, size)
        {
            this.percentPos = percentPos;
        }

        public ProgressBar(Vector2 position, Color color, uint size, bool percentPos) : base(position, color, size)
        {
            this.percentPos = percentPos;
        }

        int timer;
        public override void Draw(SpriteBatch spriteBatch)
        {
            base.Draw(spriteBatch);

            int num = (int)(((float)Progress / (float)Size) * 100f);

            string text = stepText + " ";
            string percent = num + "%";
            string loadAnimation = loadAnimFrames[(timer / 4) % 4];

            if(percentPos)
                text += percent + " ";
            else
                loadAnimation = percent + " " + loadAnimation;

            SpriteFont font = ConsoleAdventure.Font;
            spriteBatch.DrawString(font, text, Position - new Vector2(font.MeasureString(text).X + 9, 0), Color.White);
            spriteBatch.DrawString(font, loadAnimation, Position + new Vector2(SizeInPixel.X + 18, 0), Color.White);

            timer++;
        }
    }
}
