using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.Formatting
{
    /// <summary>
    /// Изменяет цвет текста, который написан в "..."<br/><br/>
    /// 
    /// Сигнатура: <c>[color:COLOR="text"]</c><br/><br/>
    /// 
    /// COLOR:
    /// <code>
    /// · aaccff             - hex
    /// · acf                - short hex
    /// · ff                 - hex gray
    /// · f                  - short hex gray
    /// · rgb~100;180;255;   - rgb
    /// · rgb~0.5;0.8;1.0;   - float rgb
    /// · g~255;             - gray
    /// · g~0.1;             - float gray
    /// · hsv~360;100;100;   - hsv
    /// · hsv~0.1;0.1;1.0;   - float hsv
    /// · std~MonoGameOrange - standard colors
    /// · ca~Rainbow         - Console Adventure colors
    /// </code>
    /// </summary>
    public class ColorFormat : IFormatMode
    {
        public Range Range { get; set; }

        public FormatTemplate Template { get; set; } = new FormatTemplate("color", 1, true);

        public string Text { get; set; }

        public int YOffset { get; set; }

        public SmartColor Color { get; set; } = new SmartColor();

        public int Define(FormatString text, List<string> arguments, string modifyText)
        {
            if (arguments == null) throw new ArgumentNullException();
            if (arguments.Count != 1) throw new ArgumentException();

            string colorArg = arguments[0];

            if (colorArg == "") return -1;

            SmartColor? color = SmartColor.ParseColor(colorArg);

            if (color.HasValue)
            {
                Color = color.Value;
            }

            int fillerLength = 0;

            for (int i = 0; i < text.String.Length; i++)
            {
                if (i >= Range.Start.Value) break;

                if (text.String[i] != '\n')
                    fillerLength++;

                else
                {
                    fillerLength = 0;
                    YOffset++;
                }
            }

            StringBuilder modifyTextBuilder = new StringBuilder();

            for (int i = 0; i < fillerLength; i++)
            {
                modifyTextBuilder.Append(" ");
            }

            modifyTextBuilder.Append(modifyText);
            Text = modifyTextBuilder.ToString();

            return text.CutFill(Range, modifyText);
        }



        public void Draw(SpriteBatch spriteBatch, Vector2 position, FormatString text)
        {
            spriteBatch.DrawString(ConsoleAdventure.Font, Text, new Vector2(position.X, position.Y + (YOffset * 19)), Color.Color);
        }
    }
}
