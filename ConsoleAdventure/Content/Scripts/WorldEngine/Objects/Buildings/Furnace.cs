using ConsoleAdventure.Content.Scripts;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Furnace : Transform
    {
        public Furnace(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)RenderFieldType.furnace;
            isObstacle = false;

            AddTypeToMap<Furnace>(type);

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new() { new Stack(new FurnaceItem(), 1) });
        }

        public override string GetSymbol()
        {
            return "[]";
        }

        public override Color GetColor()
        {
            return Color.Gray;
        }

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