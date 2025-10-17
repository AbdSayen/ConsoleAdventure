using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class Charcoal : Transform
    {
        public Charcoal(Position position, int w) : base(position, (byte)w)
        {
            worldLayer = World.BlocksLayerId;
            type = (int)VanillaTransforms.charcoal;

            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
            Hardness[type] = 0.6f;
        }

        public override string GetSymbol() => "≡≡";

        public override Color GetColor() => new(45, 45, 45);
    }
}