using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ConsoleAdventure.Content.Scripts.Player;
using ConsoleAdventure.Content.Scripts.WorldEngine.Events;
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
            public float rotation;

            public DrawableUnit(string text, Position worldPos, int w, Vector2 offset, Color color, float rotation)
            {
                this.text = text;
                this.w = w;
                this.color = color;
                position = worldPos.ToVector2() + offset;
                this.rotation = rotation;
            }
        }

        public static void Draw(string text, Position worldPos, int w, Vector2 offset, Color color, float rotation = 0)
        {
            drawableUnits.Add(new(text, worldPos, w, offset, color, rotation));
        }

        internal static void DrawUnits(Vector2 startPos)
        {
            for (int i = 0; i < GameEvent.Events.Count; i++)
            {
                if (GameEvent.Events[i].IsActive())
                {
                    GameEvent.Events[i].Draw();
                }
            }

            int playerW = ConsoleAdventure.world.GetLocalPlayer().w;
            for (int i = 0; i < drawableUnits.Count; i++)
            {
                DrawableUnit unit = drawableUnits[i];
                if (unit.w == playerW)
                {
                    Vector2 position = ((unit.position - startPos) * ConsoleAdventure.cellSize) + ConsoleAdventure.worldPos;
                    ConsoleAdventure._spriteBatch.DrawString(ConsoleAdventure.Font, unit.text, position, unit.color, unit.rotation, Vector2.Zero, 1, 0, 0); 
                }
            }
        }

        public static void Clear()
        {
            drawableUnits.Clear();
        }
    }
}
