using Microsoft.Xna.Framework;
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

            cmd.StandardInput.WriteLine("explorer \"" + absolutePath + "\"");
            cmd.StandardInput.Flush();
            cmd.StandardInput.Close();
            cmd.WaitForExit();
            ConsoleAdventure.logger.AddMessage(cmd.StandardOutput.ReadToEnd());
        }

        /// <summary>
        /// Используется для исключения искажений времени таймера, при разном fps
        /// </summary>
        /// <param name="ticks">Время в тиках, которое нужно стабилизировать</param>
        public static int StabilizeTicks(int ticks)
        {
            return (int)(ticks / ((float)(ConsoleAdventure.FPS) / 60f)); //Тут, мы умножаем тики на отношение реального фпс, и ожидаемого.
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

        public static double NextDouble(this Random random, double min, double max)
        {
            return SharpDX.RandomUtil.NextDouble(random, min, max);
        }

        public static float NextFloat(this Random random, float min, float max)
        {
            return SharpDX.RandomUtil.NextFloat(random, min, max);
        }

        public static long NextLong(this Random random, long min, long max)
        {
            return SharpDX.RandomUtil.NextLong(random, min, max);
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

        public static int HashNoise(int x, int y)
        {
            int n = x * 374761393 + y * 668265263;
            n = (n ^ (n >> 13)) * 1274126177;
            n = (n ^ (n >> 16));
            return n;
        }

        public static int HashNoise(int x, int y, int max)
        {
            int n = HashNoise(x, y);
            return Math.Abs(n % max);
        }

        public static string StringMaxLengthOnLine(string str, int maxLengthOnLine)
        {
            StringBuilder resultStr = new();
            int curThreshold = maxLengthOnLine;
            for (int i = 0; i < str.Length; i++)
            {
                if (i < curThreshold)
                    resultStr.Append(str[i]);
                else
                {
                    resultStr.Append(str[i] + "\r\n");
                    curThreshold += maxLengthOnLine;
                }
            }
            return resultStr.ToString();
        }

        public static Vector2 Move(this Vector2 position, double radians)
        {
            Vector2 vector = new Vector2(1, 0).Rotated(radians);
            Vector2 result = position + vector;
            return result;
        }

        public static Color ToColor(this Vector3 vector)
        {
            return new(vector.X, vector.Y, vector.Z);
        }

        public static Color ToColor(this Vector4 vector)
        {
            return new(vector.X, vector.Y, vector.Z, vector.W);
        }

        public static Color AddColors(Color color1, Color color2)
        {
            int R = color1.R + color2.R;
            int G = color1.G + color2.G;
            int B = color1.B + color2.B;
            return new Color
            (
                R > 255 ? 255 : R,
                G > 255 ? 255 : G,
                B > 255 ? 255 : B
            );
        }

        public static Color HexToColor(string hex)
        {
            if (string.IsNullOrEmpty(hex)) throw new ArgumentNullException(nameof(hex));
            if (hex.Length != 6 && hex.Length != 3) throw new Exception($"{hex} Hex length must be 6 or 3 characters");

            Color color = Color.White;
            string[] hexs = new string[3];
            char[] chars = hex.ToCharArray();

            if (hex.Length == 3)
            {
                StringBuilder fullHex = new StringBuilder();

                for (int i = 0; i < 6; i++)
                {
                    fullHex.Append(hex[i / 2]);
                }
                
                chars = fullHex.ToString().ToCharArray();
            }

            for (int i = 0; i < hexs.Length; i++)
            {
                hexs[i] = chars[i * 2].ToString() + chars[i * 2 + 1].ToString();
            }

            try
            {
                color.R = byte.Parse(hexs[0], System.Globalization.NumberStyles.HexNumber);
                color.G = byte.Parse(hexs[1], System.Globalization.NumberStyles.HexNumber);
                color.B = byte.Parse(hexs[2], System.Globalization.NumberStyles.HexNumber);
            }
            catch { }
            return color;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hue"></param>
        /// <param name="saturation"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Color HSVToRGB(float hue, float saturation, float value)
        {
            hue = hue % 360;
            if (hue < 0) hue += 360;

            float c = value * saturation;
            float x = c * (1 - Math.Abs((hue / 60f) % 2 - 1));
            float m = value - c;

            float r1, g1, b1;

            if (hue < 60) (r1, g1, b1) = (c, x, 0);
            else if (hue < 120) (r1, g1, b1) = (x, c, 0);
            else if (hue < 180) (r1, g1, b1) = (0, c, x);
            else if (hue < 240) (r1, g1, b1) = (0, x, c);
            else if (hue < 300) (r1, g1, b1) = (x, 0, c);
            else (r1, g1, b1) = (c, 0, x);

            byte r = (byte)Math.Round((r1 + m) * 255);
            byte g = (byte)Math.Round((g1 + m) * 255);
            byte b = (byte)Math.Round((b1 + m) * 255);

            return new Color(r, g, b);
        }

        public static int IndexOfInRange(this string str, string value, int start, int end)
        {
            if (end <= start) return -1;
            int count = end - start;
            return str.IndexOf(value, start, count);
        }

        public static Color TimeGradient(List<Color> colors, float speed = 1f)
        {
            float amount = ConsoleAdventure.Timer % 60 / 60f * speed;
            int indexColor = ConsoleAdventure.Timer / 60 % colors.Count;
            return Color.Lerp(colors[indexColor], colors[(indexColor + 1) % colors.Count], amount);
        }
    }
}
