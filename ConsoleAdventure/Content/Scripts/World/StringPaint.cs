using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleAdventure.Content.Scripts.Player;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ConsoleAdventure.WorldEngine
{
    public static class StringPaint
    {
        private static List<DrawableUnit> drawableUnits = new();

        private struct DrawableUnit
        {
            public string text;
            public Vector2 position;
            public int w;
            public Color color;

            public DrawableUnit(string text, Position worldPos, int w, Vector2 offset, Color color)
            {
                this.text = text;
                this.w = w;
                this.color = color;
                position = worldPos.ToVector2() + offset;
            }
        }

        public static void Draw(string text, Position worldPos, int w, Vector2 offset, Color color)
        {
            drawableUnits.Add(new(text, worldPos, w, offset, color));
        }

        internal static void DrawUnits(Vector2 startPos)
        {
            int playerW = ConsoleAdventure.world.GetLocalPlayer().w;
            for (int i = 0; i < drawableUnits.Count; i++)
            {
                DrawableUnit unit = drawableUnits[i];
                if (unit.w == playerW)
                {
                    ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, unit.text, ((unit.position - startPos) * ConsoleAdventure.cellSize) + ConsoleAdventure.worldPos, unit.color); 
                }
            }
        }

        public static void Clear()
        {
            drawableUnits.Clear();
        }
    }
}
