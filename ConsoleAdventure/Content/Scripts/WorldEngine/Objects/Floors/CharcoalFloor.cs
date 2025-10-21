using Microsoft.Xna.Framework;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class CharcoalFloor : Transform
    {
        public CharcoalFloor(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.charcoalFloor;
            Initialize();
        }

        public override void SetStaticData()
        {
            DefaultWorldLayer[type] = World.FloorLayerId;
        }

        public override string GetSymbol() => " .";

        public override Color GetColor() => new(35, 35, 35);
    }
}