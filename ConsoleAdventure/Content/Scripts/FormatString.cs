using ConsoleAdventure.Content.Scripts.InputLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using SharpDX.Direct3D9;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts
{
    public class FormatString
    {
        public string BaseString { get; set; } = "";

        public string String { get; set; } = "";

        public List<IFormatMode> FormatModes { get; set; } = new List<IFormatMode>();

        public Vector2 Position { get; set; } = new();

        public Color Color { get; set; } = new Color(255, 255, 255);

        public FormatString(string Text, Vector2 position, Color color)
        {
            BaseString = Text;
            String = Text;
            Position = position;
            Color = color;

            Type baseInterface = typeof(IFormatMode);
            IEnumerable<Type> list = Assembly.GetAssembly(baseInterface).GetTypes().Where(type => baseInterface.IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract);
            foreach (Type type in list)
            {
                IFormatMode mode = (IFormatMode)Activator.CreateInstance(type);

                mode.DefineAll(this);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.DrawString(ConsoleAdventure.Font, String, Position, Color);

            for (int i = 0; i < FormatModes.Count; i++)
            {
                FormatModes[i].Draw(spriteBatch, this);
            }
        }
    }

    public interface IFormatMode
    {
        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        public virtual void DefineAll(FormatString text) { }

        //public virtual void СhangeString(FormatString text) { }

        public virtual void Draw(SpriteBatch spriteBatch, FormatString text) { }
    }

    public class ColorText : IFormatMode
    {
        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        public Color Color { get; set; }

        public string Text { get; set; }

        public int Length { get; set; }

        public void DefineAll(FormatString text) //[color:******="text"]
        {
            Match match = Regex.Match(text.String, @"\[color:([a-fA-F0-9]{6})=""(.*?)""\]");

            while (match.Success)
            {
                string hexColor = match.Groups[1].Value; //Цвет (хекс)
                Color color = Utils.HexToColor(hexColor);
                string coloredText = match.Groups[2].Value; //Цветной текст

                int start = match.Groups[2].Index - 15;
                int end = match.Groups[2].Index + match.Groups[2].Length + 1;

                StringBuilder sb = new StringBuilder();

                for (int i = 0; i < text.String.Length; i++)
                {
                    if (i >= start)
                    {
                        sb.Append(coloredText);
                        break;
                    }

                    if (text.String[i] != '\n')
                        sb.Append(" ");
                    else
                        sb.Append("\n");
                }

                ColorText colorText = new ColorText()
                {
                    Color = color,
                    Text = sb.ToString(),
                    StartIndex = start,
                    EndIndex = end,
                    Length = coloredText.Length
                };

                text.FormatModes.Add(colorText);
                colorText.СhangeString(text);

                match = Regex.Match(text.String, @"\[color:([a-fA-F0-9]{6})=""(.*?)""\]"); //Ищем следующий фрагмент
            }
        }

        public void СhangeString(FormatString text) 
        {
            StringBuilder sb = new StringBuilder(text.String);
            int lengthToReplace = Math.Min(EndIndex - StartIndex + 1, sb.Length);
            sb.Remove(Math.Min(StartIndex, sb.Length), lengthToReplace);
            sb.Insert(Math.Min(StartIndex, sb.Length), new string(' ', Length));
            text.String = sb.ToString();
        }

        public void Draw(SpriteBatch spriteBatch, FormatString text)
        {
            spriteBatch.DrawString(ConsoleAdventure.Font, Text, text.Position, Color);
        }
    }

    public class ItemIcon : IFormatMode
    {
        public int StartIndex { get; set; }

        public int EndIndex { get; set; }

        public Item Item { get; set; }

        public Vector2 Offset { get; set; }

        public void DefineAll(FormatString text) //[item:itemName]
        {
            Match match = Regex.Match(text.String, @"\[item:(.*?)\]");

            while (match.Success)
            {
                string itemName = match.Groups[1].Value;

                int start = match.Groups[1].Index - 6;
                int end = match.Groups[1].Index + match.Groups[1].Length;

                Vector2 offset = new();
                for (int i = 0; i < text.String.Length; i++)
                {
                    if (i >= start) { break; }
                    if (text.String[i] == '\n') { offset.X = 0; offset.Y += 19;}
                    else if (text.String[i] != '\r') offset.X += 9;
                }    

                Item item = null;
                try { item = (Item)Activator.CreateInstance(Type.GetType(itemName)); }
                catch { }

                ItemIcon itemIcon = new ItemIcon()
                {
                    StartIndex = start,
                    EndIndex = end,
                    Item = item,
                    Offset = offset
                };

                text.FormatModes.Add(itemIcon);
                itemIcon.СhangeString(text);

                match = Regex.Match(text.String, @"\[item:(.*?)\]"); //Ищем следующий фрагмент
            }
        }

        public void СhangeString(FormatString text)
        {
            StringBuilder sb = new StringBuilder(text.String);
            int lengthToReplace = Math.Min(EndIndex - StartIndex + 1, sb.Length);
            sb.Remove(Math.Min(StartIndex, sb.Length), lengthToReplace);
            sb.Insert(Math.Min(StartIndex, sb.Length), new string(' ', 1));
            text.String = sb.ToString();
        }

        public void Draw(SpriteBatch spriteBatch, FormatString text)
        {
            if (Item == null) return;

            Item.Draw(spriteBatch, text.Position + Offset - new Vector2(0, 0));
        }
    }
}
