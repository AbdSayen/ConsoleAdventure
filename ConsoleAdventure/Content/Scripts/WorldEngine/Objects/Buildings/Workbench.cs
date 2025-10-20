using Microsoft.Xna.Framework;
using System;
using System.Collections;

namespace ConsoleAdventure.WorldEngine
{
    public class Workbench : Transform
    {
        public Workbench(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.workbench;
            Initialize();
        }

        public override void SetStaticData()
        {
            BurnType[type] = 1;
        }

        public override void Collapse() => DropItem(new WorkbenchItem());

        public override string GetSymbol() => " ∏";

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