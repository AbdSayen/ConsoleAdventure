﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Text;

namespace ConsoleAdventure
{
    public static class Utils
    {
        public static class TimeConverter
        {
            public static int SecondsToTicks(float seconds)
            {
                return (int)(seconds * 60);
            }

            public static float TicksToSeconds(int ticks)
            {
                return ((float)ticks / 60f);
            }
        }

        private static readonly char[] baseFrameChars = new char[6] {'│', '─', '┌', '┐', '└', '┘'};

        /// <summary>
        /// Создаёт рамку из символов
        /// </summary>
        /// <param name="panel">Размер рамки</param>
        /// <param name="style">Стиль рамки</param>
        /// <returns>Две строки, которые являются изображением рамки</returns>
        public static string[] GetPanel(Point panel, byte style = 0)
        {
            StringBuilder TB = new StringBuilder(); //Верх и низ
            StringBuilder LR = new StringBuilder(); //Лево и право

            char[] chars = baseFrameChars;

            for (int i = 0; i < panel.Y; i++)
            {
                for (int j = 0; j < panel.X; j++)
                {
                    if (i == 0) //Верх
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
                    else if (i == panel.Y - 1) //Низ
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
                    else //Середина
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

        /// <summary>
        /// Открывает проводник по указанному пути
        /// </summary>
        /// <param name="absolutePath">Полный путь к папке</param>
        public static void OpenExplorerAtFolder(string absolutePath)
        {
            Process cmd = new Process();
            cmd.StartInfo.FileName = "cmd.exe";
            cmd.StartInfo.RedirectStandardInput = true;
            cmd.StartInfo.RedirectStandardOutput = true;
            cmd.StartInfo.CreateNoWindow = true;
            cmd.StartInfo.UseShellExecute = false;
            cmd.Start();

            cmd.StandardInput.WriteLine("explorer " + absolutePath);
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            Console.WriteLine(cmd.StandardOutput.ReadToEnd());
        }

        /// <summary>
        /// Рисует рамку из символов
        /// </summary>
        /// <param name="font">Шрифт рамки</param>
        /// <param name="panel">Строки рамки</param>
        /// <param name="position">Позиция рамки</param>
        /// <param name="color">Цвет рамки</param>
        public static void DrawFrame(this SpriteBatch spriteBatch, SpriteFont font, string[] panel, Vector2 position, Color color)
        {
            spriteBatch.DrawString(font, panel[0], position - new Vector2(4, 0), color);
            spriteBatch.DrawString(font, panel[1], position, color);         
        }

        /// <summary>
        /// Используется для исключения искажений времени таймера, при разном fps
        /// </summary>
        /// <param name="ticks">Время в тиках, которое нужно стабилизировать</param>
        public static int StabilizeTicks(int ticks)
        {
            return (int)(ticks * (ConsoleAdventure.FPS / 60)); //Тут, мы умножаем тики на отношение реального фпс, и ожидаемого.
        }

        /// <summary>
        /// Используется для исключения искажений времени таймера, при разном fps
        /// </summary>
        /// <param name="seconds">Время в секундах, которое нужно стабилизировать</param>
        public static int StabilizeSeconds(float seconds)
        {
            return StabilizeTicks((int)(seconds * 60));
        }

        public static Position ToPosition(this Vector2 position)
        {
            return new((int)position.X, (int)position.Y);
        }

        public static Position ToPosition(this Point position)
        {
            return new(position.X, position.Y);
        }

        /// <summary>
        /// Сжимает список байтов с нужным уровнем сжатия
        /// </summary>
        /// <param name="data">Данные для сжатия</param>
        /// <param name="compressionLevel">Уровень сжатия</param>
        public static byte[] Compress(byte[] data, CompressionLevel compressionLevel)
        {
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(output, compressionLevel))
            {
                dstream.Write(data, 0, data.Length);
            }
            return output.ToArray();
        }

        /// <summary>
        /// Распаковывает ранее сжатый список байт
        /// </summary>
        /// <param name="data">Данные для распаковывки</param>
        public static byte[] Decompress(byte[] data)
        {
            MemoryStream input = new MemoryStream(data);
            MemoryStream output = new MemoryStream();
            using (DeflateStream dstream = new DeflateStream(input, CompressionMode.Decompress))
            {
                dstream.CopyTo(output);
            }
            return output.ToArray();
        }

        public static object Choose(this Random random, object[] objs)
        {
            return objs[random.Next(objs.Length)];
        }

        public static Vector2 Rotated(this Vector2 point, double radians)
        {
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            Vector2 vector = point;
            Vector2 result = Vector2.Zero;
            result.X = vector.X * cos - vector.Y * sin;
            result.Y = vector.X * sin + vector.Y * cos;
            return result;
        }
    }
}
