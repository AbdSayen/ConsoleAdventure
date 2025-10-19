using Microsoft.Xna.Framework;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class GraniteFloor : Transform
    {
        static string[] symbolsMap = new string[]
        {
            " .",
            " ~",
            " ,",
        };

        public GraniteFloor(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.graniteFloor;
            Initialize();
        }

        public override void SetStaticData()
        {
            DefaultWorldLayer[type] = World.FloorLayerId;
        }

        public override void Collapse() => DropItem(new GraniteFloorItem());

        public override string GetSymbol() => GetVariation(symbolsMap);

        public override Color GetColor() => new Color(35, 35, 35);
    }
}