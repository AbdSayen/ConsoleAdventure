using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure
{
    public static class Utils
    {
        private static readonly char[] baseFrameCars = new char[6] {'│', '─', '┌', '┐', '└', '┘'};

        /// <summary>
        /// Creates a frame
        /// </summary>
        /// <param name="panel">Frame size: width and height</param>
        /// <param name="style">Frame style</param>
        /// <returns>An "image" of a frame that you can draw</returns>
        public static string GetPanel(Point panel, byte style = 0)
        {
            StringBuilder image = new StringBuilder();
            char[] chars = baseFrameCars;

            for (int i = 0; i < panel.Y; i++)
            {
                for (int j = 0; j < panel.X; j++)
                {
                    if (i == 0) // Top
                    {
                        if (j == 0)
                            image.Append(chars[2]);
                        else if (j == panel.X - 1)
                            image.Append(chars[3]);
                        else
                            image.Append(chars[1]);
                    }
                    else if (i == panel.Y - 1) // Bottom 
                    {
                        if (j == 0)
                            image.Append(chars[4]);
                        else if (j == panel.X - 1)
                            image.Append(chars[5]);
                        else
                            image.Append(chars[1]);
                    }
                    else // Middle
                    {
                        if (j == 0 || j == panel.X - 1)
                            image.Append(chars[0]);
                        else
                            image.Append(' ');
                    }
                }
                image.Append('\n');
            }

            return image.ToString();
        }
    }
}
