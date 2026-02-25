using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class Plank : Transform
    {
        public Plank(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.log;
            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
            Hardness[type] = 0.8f;
            BurnType[type] = 1;
        }

        public override void Collapse() => DropItem(new Log(), 1, material);

        public override string GetSymbol() => GetMaterialSymbol("≡≡");

        public override Color GetColor() => GetMaterialColor();

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