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

        public override void Collapse() => DropItem(new WallItem(), 1, material);

        public override string GetSymbol() => GetMaterialSymbol("##");

        public override Color GetColor() => GetMaterialColor();
    }
}