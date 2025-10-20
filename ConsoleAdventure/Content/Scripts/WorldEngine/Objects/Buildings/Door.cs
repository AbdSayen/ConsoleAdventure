using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class Door : Transform
    {
        public Door(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.door;
            Initialize();
        }

        public override void SetStaticData()
        {
            BurnType[type] = 1;
        }

        public override void Collapse() => DropItem(new DoorItem());

        public override string GetSymbol() => "[]";

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