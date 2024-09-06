using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;

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
    }
}
