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
    /// Рисует иконку предмета на тексте "..."<br/><br/>
    /// 
    /// Сигнатура: <c>[chartex:CHARTEXTURE]</c><br/><br/>
    /// 
    /// CHARTEXTURE:
    /// <code>
    /// · add'o'#ff9900:0.0x0.0*0;add'`'#00ff00:0.0x0.0*0;
    /// 
    /// "\с" = ","
    /// "\e" = "="
    /// "\s" = "["
    /// "\bs"= "]"
    /// </code>
    /// </summary>
    public class CharTextureFormat : IFormatMode
    {
        public Range Range { get; set; }

        public FormatTemplate Template { get; set; } = new FormatTemplate("chartex", 1, false);

        public CharTexture CharTexture { get; set; }

        public Point Offset { get; set; }

        public int Define(FormatString text, List<string> arguments, string modifyText)
        {
            if (arguments == null) throw new ArgumentNullException();
            if (arguments.Count != 1) throw new ArgumentException();

            string texture = arguments[0];

            if (texture == "") return -1;

            CharTexture = CharTexture.Read(texture.Replace("\\с", ",")
                                                  .Replace("\\e", "=")
                                                  .Replace("\\s", "[")
                                                  .Replace("\\bs", "]"));

            if (CharTexture.colors.Count <= 0)
                return -1;

            int x = 0;
            int y = 0;

            for (int i = 0; i < text.String.Length; i++)
            {
                if (i >= Range.Start.Value) break;

                if (text.String[i] != '\n')
                    x++;

                else
                {
                    x = 0;
                    y++;
                }
            }

            Offset = new Point(x, y);

            return text.CutFill(Range, " ");
        }



        public void Draw(SpriteBatch spriteBatch, Vector2 position, FormatString text)
        {
            CharTexture.Draw(spriteBatch, new Vector2(position.X + (Offset.X * 9), position.Y + (Offset.Y * 19)));
        }
    }
}
