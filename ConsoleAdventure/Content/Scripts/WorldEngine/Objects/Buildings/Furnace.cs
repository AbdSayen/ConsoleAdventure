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

        public override void Collapse() => DropItem(new FurnaceItem(), 1, material);

        public override string GetSymbol() => "⌂≡";

        public override Color GetColor() => GetMaterialColor();

        int drawTimer;
        public override void OnTheScreen()
        {
            Light.Add(position.x, position.y, w, Color.White, 100f);

            if (CanDraw())
            {
                StringPaint.Draw("•", position, w, new(0.04f, 0.1f), Color.OrangeRed);
                StringPaint.Draw("·", position, w, new(0.08f + (float)(Math.Sin((float)drawTimer / 30 * Math.PI) / 20), -0.02f), Color.OrangeRed * 0.8f);
                StringPaint.Draw("·", position, w, new(0.08f, 0.1f), Color.Orange * (float)(1 + Math.Sin((float)drawTimer / 60 * Math.PI) / 4));
            }

            drawTimer++;
        }
    }
}