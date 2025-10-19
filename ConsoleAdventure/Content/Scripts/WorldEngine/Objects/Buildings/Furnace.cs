using ConsoleAdventure.Content.Scripts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;

namespace ConsoleAdventure.WorldEngine
{
    public class Furnace : Transform
    {
        public Furnace(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.furnace;
            Initialize();
        }

        public override void Collapse() => DropItem(new FurnaceItem());

        public override string GetSymbol() => "[]";

        public override Color GetColor() => Color.Gray;

        int drawTimer;
        public override void OnTheScreen()
        {
            Light.Add(position.x, position.y, w, Color.White, 100f);

            if (CanDraw())
            {
                StringPaint.Draw("●", position, w, new(0.24f, -0.00f), Color.OrangeRed);
                StringPaint.Draw("•", position, w, new(0.28f + (float)(Math.Sin((float)drawTimer / 30 * Math.PI) / 20), -0.12f), Color.OrangeRed * 0.8f);
                StringPaint.Draw("•", position, w, new(0.28f, -0.00f), Color.Orange * (float)(1 + Math.Sin((float)drawTimer / 60 * Math.PI) / 4));
            }

            drawTimer++;
        }
    }
}