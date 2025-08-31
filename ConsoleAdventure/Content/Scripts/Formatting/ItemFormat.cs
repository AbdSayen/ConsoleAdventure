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
    /// Сигнатура: <c>[item:NAME]</c><br/><br/>
    /// 
    /// NAME:
    /// <code>
    /// · IronPick            = ConsoleAdventure.IronPick
    /// · MyMod.Items.ModItem = MyMod.Items.IronPick
    /// </code>
    /// </summary>
    public class ItemFormat : IFormatMode
    {
        public Range Range { get; set; }

        public FormatTemplate Template { get; set; } = new FormatTemplate("item", 1, false);

        public Item Item { get; set; }

        public Point Offset { get; set; }

        public int Define(FormatString text, List<string> arguments, string modifyText)
        {
            if (arguments == null) throw new ArgumentNullException();
            if (arguments.Count != 1) throw new ArgumentException();

            string name = arguments[0];

            if (name == "") return -1;

            Item item = null;
            try 
            { 
                item = (Item)Activator.CreateInstance(Type.GetType(name)); 
            }

            catch { }

            if (item == null) 
                return -1;

            Item = item;

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
            Item.Draw(spriteBatch, new Vector2(position.X + (Offset.X * 9), position.Y + (Offset.Y * 19)));
        }
    }
}
