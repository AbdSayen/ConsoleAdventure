using ConsoleAdventure.Content.Scripts.InputLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.UI
{
    internal class TextInputField : BaseUI
    {
        public static byte UnderliningCursor => 0;
        public static byte VerticalStickCursor => 1;

        public string tooltip;

        public int Width;
        public int Height;
        public byte cursor;

        public Point cursorPos;

        public static int cursorTickTime = 30;

        char[] chars;
        bool listType;

        static string[] cursorChars = new string[2]
        {
            "_",
            "|"
        };

        public string[] BorderChars = new string[2]
        {
            "[",
            "]"
        };

        public TextInputField(Point position, Color color, int Width, int Height, string tooltip = "", byte cursor = 0, char[] chars = null, bool listType = false) : base(new(position, new(Width * 9, Height * 19)), color)
        {
            this.Width = Width;
            this.cursor = cursor;
            this.chars = chars;
            this.listType = listType; 
            this.tooltip = tooltip;
        }

        public void Update()
        {
            if(isHover)
            {
                text = TextInput.GetInputText(text, cursorPos, out cursorPos, chars, listType);
            }
        }

        int timer;
        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (text == "" && tooltip != "")
            {
                spriteBatch.DrawString(ConsoleAdventure.Font, tooltip, Position, Color.Gray, 0, Vector2.Zero, 1f, 0, 0);
            }

            spriteBatch.DrawString(ConsoleAdventure.Font, text, Position, color, 0, Vector2.Zero, 1f, 0, 0);

            if (isHover && timer % cursorTickTime > ((float)cursorTickTime / 2))
            {
                spriteBatch.DrawString(ConsoleAdventure.Font, cursorChars[cursor], Position + new Vector2(cursorPos.X * 9, cursorPos.Y * 19), color, 0, Vector2.Zero, 1f, 0, 0);
            }

            spriteBatch.DrawString(ConsoleAdventure.Font, BorderChars[0], Position - new Vector2(9, 0), color, 0, Vector2.Zero, 1f, 0, 0);
            spriteBatch.DrawString(ConsoleAdventure.Font, BorderChars[1], Position + new Vector2((Width + 1) * 9, 0), color, 0, Vector2.Zero, 1f, 0, 0);


            timer++;
        }
    }
}
