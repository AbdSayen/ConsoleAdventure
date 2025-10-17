using Microsoft.Xna.Framework;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class BasaltFloor : Transform
    {
        static string[] symbolsMap = new string[]
        {
            " -",
            " *",
            " “",
        };

        public BasaltFloor(Position position, int w) : base(position, (byte)w)
        {
            worldLayer = World.FloorLayerId;
            type = (int)VanillaTransforms.basaltFloor;

            Initialize();
        }

        public override void Collapse() => DropItem(new BasaltFloorItem());

        public override string GetSymbol() => GetVariation(symbolsMap);

        public override Color GetColor() => new Color(35, 35, 35);
    }
}