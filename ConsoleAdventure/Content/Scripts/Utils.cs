using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure
{
    public static class Utils
    {
        private static readonly char[] baseFrameChars = new char[6] {'│', '─', '┌', '┐', '└', '┘'};

        /// <summary>
        /// Creates a frame
        /// </summary>
        /// <param name="panel">Frame size: width and height</param>
        /// <param name="style">Frame style</param>
        /// <returns>An "image" of a frame that you can draw</returns>
        public static string[] GetPanel(Point panel, byte style = 0)
        {
            StringBuilder TB = new StringBuilder(); //Top and Bottom
            StringBuilder LR = new StringBuilder(); //Left and Right

            char[] chars = baseFrameChars;

            for (int i = 0; i < panel.Y; i++)
            {
                for (int j = 0; j < panel.X; j++)
                {
                    if (i == 0) // Top
                    {
                        if (j == 0) 
                        { 
                            TB.Append(' '); 
                            LR.Append(chars[2]); 
                        }
                        else if (j == panel.X - 1) 
                        { 
                            TB.Append(chars[3]); 
                            LR.Append(' '); 
                        }
                        else 
                        { 
                            TB.Append(chars[1]);
                            LR.Append(' ');
                        }
                    }
                    else if (i == panel.Y - 1) // Bottom 
                    {
                        if (j == 0)
                        {
                            TB.Append(' ');
                            LR.Append(chars[4]);
                        }
                        else if (j == panel.X - 1)
                        {
                            TB.Append(chars[5]);
                            LR.Append(' ');
                        }
                        else
                        {
                            TB.Append(chars[1]);
                            LR.Append(' ');
                        }
                    }
                    else // Middle
                    {
                        if (j == 0 || j == panel.X - 1)
                        {
                            TB.Append(' ');
                            LR.Append(chars[0]);
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
}
