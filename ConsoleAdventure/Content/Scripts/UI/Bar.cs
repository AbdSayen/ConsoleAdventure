using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI
{
    public class Bar : BaseUI
    {
        public char baseSymbol = '.';
        public char newSymbol = '▪';

        private uint progress;

        public Vector2 SizeInPixel { get; private set; }

        public uint Size { get; private set; }

        public uint Progress
        {
            get
            {
                return progress;
            }

            set
            {
                if (value > Size)
                    progress = Size;
                else
                    progress = value;
            }
        }

        public Bar(Rectangle rectangle, Color color, uint size) : base(rectangle, color, false)
        {
            this.Size = size;
        }

        public Bar(Vector2 position, Color color, uint size) : base(new Rectangle((int)position.X, (int)position.Y, (int)(size * 9), 19), color)
        {
            this.Size = size;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            string oldBar = "";
            string newBar = "";

            for(int i = 0; i < Size; i++)
            {
                if(i <= progress)
                {
                    oldBar += " ";
                    newBar += newSymbol;
                }

                else
                {
                    oldBar += baseSymbol;
                }
            }

            SpriteFont font = ConsoleAdventure.Font;

            SizeInPixel = font.MeasureString(oldBar); 

            spriteBatch.DrawString(font, "[", Position - new Vector2(9, 0), Color.White);
            spriteBatch.DrawString(font, oldBar, Position, Color.Gray);
            spriteBatch.DrawString(font, newBar, Position, color);
            spriteBatch.DrawString(font, "]", Position + new Vector2(SizeInPixel.X, 0), Color.White);
        }
    }
}
