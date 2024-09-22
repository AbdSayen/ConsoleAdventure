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
        private static readonly char[] baseFrameChars = new char[6] { '│', '─', '┌', '┐', '└', '┘' };
                                                                  //{  1 ,  0 ,  1 ,  0 ,  1 ,  0  }

        /// <summary>
        /// Создаёт рамку из символов
        /// </summary>
        /// <param name="panel">Размер рамки</param>
        /// <param name="style">Стиль рамки</param>
        /// <returns>Две строки, которые являются изображением рамки</returns>
        public static string[] GetFrame(Point panel, Frame style)
        {
            StringBuilder TB = new StringBuilder(); //Верх и низ
            StringBuilder LR = new StringBuilder(); //Лево и право

            char[] left = style.GetDistributedSymbol(0, false);
            char[] top = style.GetDistributedSymbol(1, false);
            char[] right = style.GetDistributedSymbol(0, true);
            char[] bottom = style.GetDistributedSymbol(1, true);
            char[] leftTop = style.GetDistributedSymbol(2, false);
            char[] rightTop = style.GetDistributedSymbol(3, false);
            char[] leftBottom = style.GetDistributedSymbol(4, false);
            char[] rightBottom = style.GetDistributedSymbol(5, false);

            for (int i = 0; i < panel.Y; i++)
            {
                for (int j = 0; j < panel.X; j++)
                {
                    if (i == 0) //Верх
                    {
                        if (j == 0)
                        {
                            TB.Append(leftTop[0]);
                            LR.Append(leftTop[1]);
                        }
                        else if (j == panel.X - 1)
                        {
                            TB.Append(rightTop[0]);
                            LR.Append(rightTop[1]);
                        }
                        else
                        {
                            TB.Append(top[0]);
                            LR.Append(top[1]);
                        }
                    }
                    else if (i == panel.Y - 1) //Низ
                    {
                        if (j == 0)
                        {
                            TB.Append(leftBottom[0]);
                            LR.Append(leftBottom[1]);
                        }
                        else if (j == panel.X - 1)
                        {
                            TB.Append(rightBottom[0]);
                            LR.Append(rightBottom[1]);
                        }
                        else
                        {
                            TB.Append(bottom[0]);
                            LR.Append(bottom[1]);
                        }
                    }
                    else //Середина
                    {
                        if (j == 0)
                        {
                            TB.Append(left[0]);
                            LR.Append(left[1]);
                        }
                        if (j == panel.X - 1)
                        {
                            TB.Append(right[0]);
                            LR.Append(right[1]);
                        }
                        else
                        {
                            TB.Append(' ');
                            LR.Append(' ');
                        }
                    }
                }
                TB.Append('\n');
                LR.Append('\n');
            }

            return new string[2] { TB.ToString(), LR.ToString(), };
        }

        public static void DrawFrame(this SpriteBatch spriteBatch, SpriteFont font, string[] panel, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, panel[0], position - new Vector2(4, 0), color);
            spriteBatch.DrawString(font, panel[1], position, color);
        }
    }

    public struct Frame
    {
        public char[] chars = new char[6];
        public bool[] distributions = new bool[6];
        public char? rightVariant = null;
        public char? bottomVariant = null;

        public Frame(char[] chars, bool[] distributions, char? rightVariant = null, char? bottomVariant = null)
        {
            if (chars == null || distributions == null)
                throw new ArgumentNullException();

            if (chars.Length != 6)
                throw new ArgumentException("chars length must be equals 6");

            if (distributions.Length != 6)
                throw new ArgumentException("distributions length must be equals 6");

            this.chars = chars;
            this.distributions = distributions;
            this.rightVariant = rightVariant;
            this.bottomVariant = bottomVariant;
        }

        public char[] GetDistributedSymbol(byte index, bool isAlt)
        {
            if(index < 0 || index >= chars.Length) throw new ArgumentOutOfRangeException("index");

            char curChar = chars[index];

            if(index == 0 && isAlt && rightVariant != null)
                curChar = rightVariant.Value;

            if (index == 1 && isAlt && bottomVariant != null)
                curChar = bottomVariant.Value;

            if (distributions[index])
                return new char[] {' ',  curChar};

            return new char[] { curChar, ' ' };
        }

        #region Properties 

        public static bool[] StandardDistributions { get; } = new bool[6] { true, false, true, false, true, false };
        public static bool[] MonoDistributions { get; } = new bool[6] { true, true, true, true, true, true };

        public static Frame BaseFrame     { get; } = new Frame(new char[6] { '│', '─', '┌', '┐', '└', '┘' }, StandardDistributions);
        public static Frame BaseFrame1Dot { get; } = new Frame(new char[6] { '╎', '╌', '┌', '┐', '└', '┘' }, StandardDistributions);
        public static Frame BaseFrame2Dot { get; } = new Frame(new char[6] { '┆', '┄', '┌', '┐', '└', '┘' }, StandardDistributions);
        public static Frame BaseFrame3Dot { get; } = new Frame(new char[6] { '┊', '┈', '┌', '┐', '└', '┘' }, StandardDistributions);

        public static Frame VerticalThicknessFrame     { get; } = new Frame(new char[6] { '│', '━', '┍', '┑', '┕', '┙' }, StandardDistributions);
        public static Frame VerticalThicknessFrame1Dot { get; } = new Frame(new char[6] { '╎', '╍', '┍', '┑', '┕', '┙' }, StandardDistributions);
        public static Frame VerticalThicknessFrame2Dot { get; } = new Frame(new char[6] { '┆', '┅', '┍', '┑', '┕', '┙' }, StandardDistributions);
        public static Frame VerticalThicknessFrame3Dot { get; } = new Frame(new char[6] { '┊', '┉', '┍', '┑', '┕', '┙' }, StandardDistributions);

        public static Frame HorizontalThicknessFrame     { get; } = new Frame(new char[6] { '┃', '─', '┎', '┒', '┖', '┚' }, StandardDistributions);
        public static Frame HorizontalThicknessFrame1Dot { get; } = new Frame(new char[6] { '╏', '─', '┎', '┒', '┖', '┚' }, StandardDistributions);
        public static Frame HorizontalThicknessFrame2Dot { get; } = new Frame(new char[6] { '┇', '─', '┎', '┒', '┖', '┚' }, StandardDistributions);
        public static Frame HorizontalThicknessFrame3Dot { get; } = new Frame(new char[6] { '┋', '─', '┎', '┒', '┖', '┚' }, StandardDistributions);

        public static Frame ThicknessFrame     { get; } = new Frame(new char[6] { '┃', '━', '┏', '┓', '┗', '┛' }, StandardDistributions);
        public static Frame ThicknessFrame1Dot { get; } = new Frame(new char[6] { '╏', '╍', '┏', '┓', '┗', '┛' }, StandardDistributions);
        public static Frame ThicknessFrame2Dot { get; } = new Frame(new char[6] { '┇', '┅', '┏', '┓', '┗', '┛' }, StandardDistributions);
        public static Frame ThicknessFrame3Dot { get; } = new Frame(new char[6] { '┋', '┉', '┏', '┓', '┗', '┛' }, StandardDistributions);

        public static Frame RoundedFrame     { get; } = new Frame(new char[6] { '│', '─', '╭', '╮', '╰', '╯' }, StandardDistributions);
        public static Frame RoundedFrame1Dot { get; } = new Frame(new char[6] { '╎', '╌', '╭', '╮', '╰', '╯' }, StandardDistributions);
        public static Frame RoundedFrame2Dot { get; } = new Frame(new char[6] { '┆', '┄', '╭', '╮', '╰', '╯' }, StandardDistributions);
        public static Frame RoundedFrame3Dot { get; } = new Frame(new char[6] { '┊', '┈', '╭', '╮', '╰', '╯' }, StandardDistributions);

        public static Frame VerticalDoubleFrame     { get; } = new Frame(new char[6] { '│', '═', '╒', '╕', '╘', '╛' }, StandardDistributions);
        public static Frame VerticalDoubleFrame1Dot { get; } = new Frame(new char[6] { '╎', '═', '╒', '╕', '╘', '╛' }, StandardDistributions);
        public static Frame VerticalDoubleFrame2Dot { get; } = new Frame(new char[6] { '┆', '═', '╒', '╕', '╘', '╛' }, StandardDistributions);
        public static Frame VerticalDoubleFrame3Dot { get; } = new Frame(new char[6] { '┊', '═', '╒', '╕', '╘', '╛' }, StandardDistributions);

        public static Frame HorizontalDoubleFrame   { get; } = new Frame(new char[6] { '║', '─', '╓', '╖', '╙', '╜' }, StandardDistributions);
        public static Frame HorizontalDoubleFrame1Dot { get; } = new Frame(new char[6] { '║', '╌', '╓', '╖', '╙', '╜' }, StandardDistributions);
        public static Frame HorizontalDoubleFrame2Dot { get; } = new Frame(new char[6] { '║', '┄', '╓', '╖', '╙', '╜' }, StandardDistributions);
        public static Frame HorizontalDoubleFrame3Dot { get; } = new Frame(new char[6] { '║', '┈', '╓', '╖', '╙', '╜' }, StandardDistributions);

        public static Frame DoubleFrame { get; } = new Frame(new char[6] { '║', '═', '╔', '╗', '╚', '╝' }, StandardDistributions);

        public static Frame MonoFrame { get; } = new Frame(new char[6] { '█', '█', '█', '█', '█', '█' }, MonoDistributions);

        public static Frame MonoThicknessFrame { get; } = new Frame(new char[6] { '█', '▄', '▄', '▄', '▀', '▀' }, MonoDistributions, bottomVariant: '▀');

        public static Frame MonoThicknessRoundedFrame { get; } = new Frame(new char[6] { '█', '▄', ' ', ' ', ' ', ' ' }, MonoDistributions, bottomVariant: '▀');

        public static Frame OldFrame { get; } = new Frame(new char[6] { '|', '-', '+', '+', '+', '+' }, MonoDistributions);

        #endregion
    }
}
