using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class Wall : Transform
    {
        public Wall(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.wall;
            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
        }

        public override void Collapse() => DropItem(new WallItem());

        public override string GetSymbol() => "##";

        public override Color GetColor() => Color.White;
    }
}