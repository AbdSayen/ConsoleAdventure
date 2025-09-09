using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts
{
    public static class FrameSystem
    {
        /// <summary>
        /// Создаёт рамку из символов
        /// </summary>
        /// <param name="panel">Размер рамки</param>
        /// <param name="style">Стиль рамки</param>
        /// <returns>Две строки, которые являются изображением рамки</returns>
        public static string GetFrame(Point panel, Frame style)
        {
            StringBuilder sb = new StringBuilder(); //Верх и низ

            char left = style.GetSymbol(0, false);
            char top = style.GetSymbol(1, false);
            char right = style.GetSymbol(0, true);
            char bottom = style.GetSymbol(1, true);
            char leftTop = style.GetSymbol(2, false);
            char rightTop = style.GetSymbol(3, false);
            char leftBottom = style.GetSymbol(4, false);
            char rightBottom = style.GetSymbol(5, false);

            for (int i = 0; i < panel.Y; i++)
            {
                for (int j = 0; j < panel.X; j++)
                {
                    if (i == 0) //Верх
                    {
                        if (j == 0)
                            sb.Append(leftTop);

                        else if (j == panel.X - 1)
                            sb.Append(rightTop);

                        else
                            sb.Append(top);
                    }
                    else if (i == panel.Y - 1) //Низ
                    {
                        if (j == 0)
                            sb.Append(leftBottom);

                        else if (j == panel.X - 1)
                            sb.Append(rightBottom);

                        else
                            sb.Append(bottom);
                    }
                    else //Середина
                    {
                        if (j == 0)
                            sb.Append(left);

                        else if (j == panel.X - 1)
                            sb.Append(right);

                        else
                            sb.Append(' ');
                    }
                }
                sb.Append('\n');
            }

            return sb.ToString();
        }
    }

    public struct Frame
    {
        public char[] chars = new char[6];
        public char? rightVariant = null;
        public char? bottomVariant = null;

        public Frame(char[] chars, char? rightVariant = null, char? bottomVariant = null)
        {
            if (chars == null)
                throw new ArgumentNullException();

            if (chars.Length != 6)
                throw new ArgumentException("chars length must be equals 6");

            this.chars = chars;
            this.rightVariant = rightVariant;
            this.bottomVariant = bottomVariant;
        }

        public char GetSymbol(byte index, bool isAlt)
        {
            if (index < 0 || index >= chars.Length) throw new ArgumentOutOfRangeException("index");

            char curChar = chars[index];

            if (index == 0 && isAlt && rightVariant != null)
                curChar = rightVariant.Value;

            if (index == 1 && isAlt && bottomVariant != null)
                curChar = bottomVariant.Value;

            return curChar;
        }

        #region Properties 

        public static Frame BaseFrame     { get; } = new Frame(new char[6] { '│', '─', '┌', '┐', '└', '┘' });
        public static Frame BaseFrame1Dot { get; } = new Frame(new char[6] { '╎', '╌', '┌', '┐', '└', '┘' });
        public static Frame BaseFrame2Dot { get; } = new Frame(new char[6] { '┆', '┄', '┌', '┐', '└', '┘' });
        public static Frame BaseFrame3Dot { get; } = new Frame(new char[6] { '┊', '┈', '┌', '┐', '└', '┘' });

        public static Frame HorizontalWideFrame { get; } = new Frame(new char[6] { '│', '━', '┍', '┑', '┕', '┙' });
        public static Frame HorizontalWideFrame1Dot { get; } = new Frame(new char[6] { '╎', '╍', '┍', '┑', '┕', '┙' });
        public static Frame HorizontalWideFrame2Dot { get; } = new Frame(new char[6] { '┆', '┅', '┍', '┑', '┕', '┙' });
        public static Frame HorizontalWideFrame3Dot { get; } = new Frame(new char[6] { '┊', '┉', '┍', '┑', '┕', '┙' });

        public static Frame VerticalWideFrame { get; } = new Frame(new char[6] { '┃', '─', '┎', '┒', '┖', '┚' });
        public static Frame VerticalWideFrame1Dot { get; } = new Frame(new char[6] { '╏', '─', '┎', '┒', '┖', '┚' });
        public static Frame VerticalWideFrame2Dot { get; } = new Frame(new char[6] { '┇', '─', '┎', '┒', '┖', '┚' });
        public static Frame VerticalWideFrame3Dot { get; } = new Frame(new char[6] { '┋', '─', '┎', '┒', '┖', '┚' });

        public static Frame WideFrame     { get; } = new Frame(new char[6] { '┃', '━', '┏', '┓', '┗', '┛' });
        public static Frame WideFrame1Dot { get; } = new Frame(new char[6] { '╏', '╍', '┏', '┓', '┗', '┛' });
        public static Frame WideFrame2Dot { get; } = new Frame(new char[6] { '┇', '┅', '┏', '┓', '┗', '┛' });
        public static Frame WideFrame3Dot { get; } = new Frame(new char[6] { '┋', '┉', '┏', '┓', '┗', '┛' });

        public static Frame RoundedFrame     { get; } = new Frame(new char[6] { '│', '─', '╭', '╮', '╰', '╯' });
        public static Frame RoundedFrame1Dot { get; } = new Frame(new char[6] { '╎', '╌', '╭', '╮', '╰', '╯' });
        public static Frame RoundedFrame2Dot { get; } = new Frame(new char[6] { '┆', '┄', '╭', '╮', '╰', '╯' });
        public static Frame RoundedFrame3Dot { get; } = new Frame(new char[6] { '┊', '┈', '╭', '╮', '╰', '╯' });

        public static Frame HorizontalDoubleFrame { get; } = new Frame(new char[6] { '│', '═', '╒', '╕', '╘', '╛' });
        public static Frame HorizontalDoubleFrame1Dot { get; } = new Frame(new char[6] { '╎', '═', '╒', '╕', '╘', '╛' });
        public static Frame HorizontalDoubleFrame2Dot { get; } = new Frame(new char[6] { '┆', '═', '╒', '╕', '╘', '╛' });
        public static Frame HorizontalDoubleFrame3Dot { get; } = new Frame(new char[6] { '┊', '═', '╒', '╕', '╘', '╛' });

        public static Frame VerticalDoubleFrame { get; } = new Frame(new char[6] { '║', '─', '╓', '╖', '╙', '╜' });
        public static Frame VerticalDoubleFrame1Dot { get; } = new Frame(new char[6] { '║', '╌', '╓', '╖', '╙', '╜' });
        public static Frame VerticalDoubleFrame2Dot { get; } = new Frame(new char[6] { '║', '┄', '╓', '╖', '╙', '╜' });
        public static Frame VerticalDoubleFrame3Dot { get; } = new Frame(new char[6] { '║', '┈', '╓', '╖', '╙', '╜' });

        public static Frame DoubleFrame { get; } = new Frame(new char[6] { '║', '═', '╔', '╗', '╚', '╝' });

        public static Frame MonoFrame { get; } = new Frame(new char[6] { '█', '█', '█', '█', '█', '█' });

        public static Frame MonoWideFrame { get; } = new Frame(new char[6] { '█', '▄', '▄', '▄', '▀', '▀' }, bottomVariant: '▀');

        public static Frame MonoWideRoundedFrame { get; } = new Frame(new char[6] { '█', '▄', ' ', ' ', ' ', ' ' }, bottomVariant: '▀');

        #endregion
    }
}
