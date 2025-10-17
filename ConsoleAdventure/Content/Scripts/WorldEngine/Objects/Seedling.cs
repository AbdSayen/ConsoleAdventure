using ConsoleAdventure.Content.Scripts;
using Microsoft.Xna.Framework;
using SharpDX.Direct3D11;
using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleAdventure.WorldEngine
{
    public class Seedling : Transform
    {
        public Seedling(Position position, int w) : base(position, (byte)w)
        {
            worldLayer = World.BlocksLayerId;
            type = (int)VanillaTransforms.seedling;

            Initialize();
        }

        public override void SetStaticData() => BurnType[type] = 1;

        public override string GetSymbol() => " ╢";

        public override Color GetColor() => new(94, 61, 38);

        public override void OnTheScreen()
        {
            Color color = new Color(13, 152, 20) * 0.5f;
            color.A = 255;

            Position tryColorPos = new Position(Math.Clamp(position.x - ConsoleAdventure.startDisplay.x, 0, 60), Math.Clamp(position.y - ConsoleAdventure.startDisplay.y, 0, 30));
            Color crownColor = (color.ToVector3() * Light.colors[tryColorPos.x, tryColorPos.y].ToVector3()).ToColor();

            if (crownColor.R != 0 && crownColor.G != 0 && crownColor.B != 0 && CanDraw()) 
            { 
                StringPaint.Draw("¾", position, w, new(), crownColor);
            }
        }

        public override void RandomUpdate()
        {
            //new Tree(position, w);
        }

        public override void AfterBurning()
        {
            if (ConsoleAdventure.rand.Next(0, 2) == 1)
            {
                new Charcoal(position, w);
            }


            else
            {
                base.AfterBurning();
            }
        }
    }
}