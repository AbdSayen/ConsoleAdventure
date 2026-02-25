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

        public override void Collapse() => DropItem(new DoorItem(), 1, material);

        public override string GetSymbol() => GetMaterialSymbol("[]");

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