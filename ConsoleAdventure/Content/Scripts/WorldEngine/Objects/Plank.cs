using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class Plank : Transform
    {
        public Plank(Position position, int w) : base(position, (byte)w)
        {
            worldLayer = World.BlocksLayerId;
            type = (int)VanillaTransforms.log;

            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
            Hardness[type] = 0.8f;
            BurnType[type] = 1;
        }

        public override void Collapse() => DropItem(new Log());

        public override string GetSymbol() => "≡≡";

        public override Color GetColor() => new(94, 61, 38);

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