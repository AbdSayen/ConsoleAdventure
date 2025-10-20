using Microsoft.Xna.Framework;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class WoodFloor : Transform
    {
        public WoodFloor(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.woodFloor;
            Initialize();
        }

        public override void SetStaticData()
        {
            DefaultWorldLayer[type] = World.FloorLayerId;
            BurnType[type] = 1;
        }

        public override void Collapse() => DropItem(new WoodFloorItem());

        public override string GetSymbol() => " .";

        public override Color GetColor() => new(74, 41, 18);

        public override void AfterBurning()
        {
            if (ConsoleAdventure.rand.Next(0, 2) == 1)
            {
                new CharcoalFloor(position, w);
            }

            else
            {
                base.AfterBurning();
            }
        }
    }
}