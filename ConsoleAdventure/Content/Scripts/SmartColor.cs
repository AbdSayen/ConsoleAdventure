using CaModLoaderAPI;
using ConsoleAdventure.Content.Scripts.InputLogic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts
{
    public struct SmartColor
    {
        private Color color;
        private int customColor = -1;

        public Color Color 
        { 
            get 
            {
                if (customColor >= 0 && customColor < CustomColorLoader.Colors.Count)
                    return CustomColorLoader.Colors.ElementAt(customColor).Value.Invoke();
                
                return color;
            }
        }

        public SmartColor()
        {
            color = Color.White;
            customColor = -1;
        }

        public SmartColor(byte r, byte g, byte b)
        {
            color = new Color(r, g, b);
            customColor = -1;
        }

        public SmartColor(float r, float g, float b)
        {
            color = new Color(r, g, b);
            customColor = -1;
        }

        public SmartColor(byte gray)
        {
            color = new Color(gray, gray, gray);
            customColor = -1;
        }

        public SmartColor(float gray)
        {
            color = new Color(gray, gray, gray);
            customColor = -1;
        }

        public SmartColor(string hex)
        {
            color = Color.White;
            customColor = -1;

            if (hex.Length != 1 && hex.Length != 2 && hex.Length != 3 && hex.Length != 6)
                return;

            StringBuilder hexBuilder = new(hex);

            if (hex.Length >= 1 && hex.Length <= 2)
            {
                hexBuilder.Append(hex);
                hexBuilder.Append(hex);
            }

            color = Utils.HexToColor(hexBuilder.ToString());
        }

        public static SmartColor? GetNameColor(string name, ColorNameMode mode)
        {
            if (mode == ColorNameMode.Default)
            {
                PropertyInfo colorInfo = typeof(Color).GetProperty(name, BindingFlags.Static | BindingFlags.Public);

                if (colorInfo != null)
                {
                    object colorData = colorInfo.GetValue(null);

                    if (colorData != null && colorData is Color)
                    {
                        return new SmartColor((Color)colorData);
                    }
                }

                return null;
            }

            else if (CustomColorLoader.Colors.ContainsKey(name))
            {
                SmartColor newColor = new SmartColor();
                newColor.customColor = CustomColorLoader.Colors.Keys.ToList().IndexOf(name);

                return newColor;
            }

            return null;
        }

        public static SmartColor GetHSVColor(int hue, int saturation, int value)
        {
            float s = Math.Clamp(saturation / 100f, 0f, 1f);
            float v = Math.Clamp(value / 100f, 0f, 1f);

            return new SmartColor(Utils.HSVToRGB(hue, s, v));
        }

        public SmartColor(Color color)
        {
            this.color = color;
            customColor = -1;
        }

        public static SmartColor? ParseColor(string colorText)
        {
            if (colorText.Length >= 10 && colorText.IndexOf("rgb~") == 0)
            {
                List<string> values = new List<string>();
                int valueIndex = 0;

                values.Add("");

                for (int i = 4; i < colorText.Length; i++)
                {
                    char c = colorText[i];

                    if (c != ';')
                        values[valueIndex] += c;

                    else
                    {
                        valueIndex++;
                        if (values.Count < 3)
                            values.Add("");
                    }
                }

                byte[] bytes = new byte[values.Count];
                bool isByte = true;

                for (int i = 0; i < values.Count; i++)
                {
                    if (byte.TryParse(values[i], out byte result))
                        bytes[i] = result;

                    else { isByte = false; break; }
                }

                float[] floats = new float[values.Count];
                bool isFloat = true;

                for (int i = 0; i < values.Count; i++)
                {
                    if (float.TryParse(values[i].Replace('.', ','), out float result))
                        floats[i] = result;

                    else { isFloat = false; break; }
                }

                if (isByte)
                {
                    return new SmartColor(bytes[0], bytes[1], bytes[2]);
                }

                else if (isFloat)
                {
                    return new SmartColor(floats[0], floats[1], floats[2]);
                }

                return null;
            }

            else if (colorText.Length >= 10 && colorText.IndexOf("hsv~") == 0)
            {
                List<string> values = new List<string>();
                int valueIndex = 0;

                values.Add("");

                for (int i = 4; i < colorText.Length; i++)
                {
                    char c = colorText[i];

                    if (c != ';')
                        values[valueIndex] += c;

                    else
                    {
                        valueIndex++;
                        if (values.Count < 3)
                            values.Add("");
                    }
                }

                int[] ints = new int[values.Count];
                bool isInt = true;

                for (int i = 0; i < values.Count; i++)
                {
                    if (int.TryParse(values[i], out int result))
                        ints[i] = result;

                    else { isInt = false; break; }
                }

                float[] floats = new float[values.Count];
                bool isFloat = true;

                for (int i = 0; i < values.Count; i++)
                {
                    if (float.TryParse(values[i].Replace('.', ','), out float result))
                        floats[i] = result;

                    else { isFloat = false; break; }
                }

                if (isInt)
                {
                    return GetHSVColor(ints[0], ints[1], ints[2]);
                }

                else if (isFloat)
                {
                    return GetHSVColor((int)(floats[0] * 360), (int)(floats[1] * 100f), (int)(floats[2] * 100f));
                }

                return null;
            }

            else if (colorText.Length >= 4 && colorText.IndexOf("g~") == 0)
            {
                string value = "";

                for (int i = 2; i < colorText.Length; i++)
                {
                    char c = colorText[i];

                    if (c != ';')
                        value += c;
                }

                byte byteGray = 0;
                bool isByte = true;

                if (byte.TryParse(value, out byte result))
                    byteGray = result;

                else { isByte = false; }

                float floatGray = 0;
                bool isFloat = true;

                if (float.TryParse(value.Replace('.', ','), out float result1))
                    floatGray = result1;

                else { isFloat = false; }

                if (isByte)
                {
                    return new SmartColor(byteGray);
                }

                else if (isFloat)
                {
                    return new SmartColor(floatGray);
                }

                return null;
            }

            else if (colorText.Length >= 5 && colorText.IndexOf("std~") == 0)
            {
                string value = "";

                for (int i = 4; i < colorText.Length; i++)
                {
                    value += colorText[i];
                }

                return GetNameColor(value, ColorNameMode.Default);
            }

            else if (colorText.Length >= 4 && colorText.IndexOf("ca~") == 0)
            {
                string value = "";

                for (int i = 3; i < colorText.Length; i++)
                {
                    value += colorText[i];
                }

                return GetNameColor(value, ColorNameMode.Custom);
            }

            return new SmartColor(colorText);
        }
    }

    public static class CustomColorLoader
    {
        public static Dictionary<string, Func<Color>> Colors { get; } = new();

        public static void Init()
        {
            Colors.Clear();
            Colors.Add("Wood", () => ColorAssets.woodenColor);
            Colors.Add("Granite", () => ColorAssets.graniteColor);
            Colors.Add("TheLion", () => ColorAssets.theLionColor);
            Colors.Add("Rainbow", () => 
            {
                float hue = (ConsoleAdventure.Timer % 360);
                return Utils.HSVToRGB(hue, 1f, 1f);
            });
            Colors.Add("BlackWhite", () =>
            {
                List<Color> colors = new List<Color>() { Color.Black, Color.White };
                return Utils.TimeGradient(colors, 1f);
            });
        }
    }

    public enum ColorNameMode
    {
        Default,
        Custom
    }
}
