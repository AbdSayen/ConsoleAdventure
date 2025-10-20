using Microsoft.Xna.Framework;
using System;

namespace ConsoleAdventure.WorldEngine
{
    public class Water : Transform
    {
        public Water(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.water;
            Initialize();
        }

        public override void SetStaticData()
        {
            //IsObstacle[type] = true;
            Hardness[type] = -1f;
        }

        public override string GetSymbol() => "≈≈";

        public override Color GetColor() => new(16, 29, 211);
    }
}