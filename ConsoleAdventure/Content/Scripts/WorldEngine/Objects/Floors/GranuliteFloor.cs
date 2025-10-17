using Microsoft.Xna.Framework;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class GranuliteFloor : Transform
    {
        static string[] symbolsMap = new string[]
        {
            " -",
            " ~",
            " <",
        };

        public GranuliteFloor(Position position, int w) : base(position, (byte)w)
        {
            worldLayer = World.FloorLayerId;
            type = (int)VanillaTransforms.granuliteFloor;

            Initialize();
        }

        public override void Collapse() => DropItem(new GranuliteFloorItem());

        public override string GetSymbol() => GetVariation(symbolsMap);

        public override Color GetColor() => new Color(100, 100, 73);
    }
}