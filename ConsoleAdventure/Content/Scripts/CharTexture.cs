using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace ConsoleAdventure.Content.Scripts
{
    public struct CharTexture
    {
        public List<string> strings;
        public List<Color> colors;
        public List<Vector2> offsets;
        public List<float> rotations;

        public CharTexture(List<string> strings, List<Color> colors, List<Vector2> offsets, List<float> rotations)
        {
            this.strings = strings;

            int count = strings.Count;

            if (count != colors.Count)
                throw new ArgumentException("colors count differs from string count");

            if (count != offsets.Count)
                throw new ArgumentException("offsets count differs from string count");

            if (count != rotations.Count)
                throw new ArgumentException("rotations count differs from string count");

            this.colors = colors;
            this.offsets = offsets;
            this.rotations = rotations;
        }

        public CharTexture()
        {
            strings = new List<string>();
            colors = new List<Color>();
            offsets = new List<Vector2>();
            rotations = new List<float>();
        }

        public CharTexture AddLayer(string text, Color color, Vector2 offset, float rotation) 
        { 
            strings.Add(text);
            colors.Add(color);
            offsets.Add(offset);
            rotations.Add(rotation);
            return this;
        }

        public CharTexture AddLayer(string text, Color color, Vector2 offset)
        {
            AddLayer(text, color, offset, 0);
            return this;
        }

        public CharTexture AddLayer(string text, Color color)
        {
            AddLayer(text, color, new());
            return this;
        }

        public CharTexture AddLayer(string text)
        {
            AddLayer(text, Color.White);
            return this;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 position)
        {
            for (int i = 0; i < strings.Count; i++)
            {
                spriteBatch.DrawString(ConsoleAdventure.Font, strings[i], position + offsets[i], colors[i], rotations[i], new(), 1f, 0, 0);
            }
        }

        public static CharTexture Read(string text)
        {
            CharTexture texture = new CharTexture();

            List<string> layers = new();

            StringBuilder buffer = new();
            for (int i = 0; i < text.Length; i++)
            {
                buffer.Append(text[i]);
                if(text[i] == ';')
                {
                    layers.Add(buffer.ToString());
                    buffer.Clear();
                }
            }

            for (int i = 0; i < layers.Count; i++)
            {
                try
                {
                    Match match = Regex.Match(layers[i], @"add'(.*?)'#([a-fA-F0-9]{6}):(.*?)x(.*?)\*(.*?);", RegexOptions.Singleline);

                    string chars = match.Groups[1].Value.Replace("\\n", "\n");
                    Color color = Utils.HexToColor(match.Groups[2].Value);

                    float x = float.Parse(match.Groups[3].Value.Replace('.', ','));
                    float y = float.Parse(match.Groups[4].Value.Replace('.', ','));

                    float rotation = float.Parse(match.Groups[5].Value.Replace('.', ','));

                    texture.AddLayer(chars, color, new(x, y), rotation);
                }

                catch { }
            }

            return texture;
        }
    }
}
