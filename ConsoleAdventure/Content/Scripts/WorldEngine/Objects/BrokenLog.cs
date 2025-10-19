using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure.WorldEngine
{
    public class BrokenLog : Transform
    {
        public BrokenLog(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.brokenLog;
            Initialize();
        }

        public override void SetStaticData()
        {
            BurnType[type] = 1;
        }

        public override void Collapse()
        {
            if(ConsoleAdventure.rand.Next(0, 3) == 0)
            {
                DropItem(new Log());
            }
        }

        public override string GetSymbol() => "⸗₋";

        public override Color GetColor() => new(74, 41, 18);

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