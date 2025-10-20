using CaModLoaderAPI;
using ConsoleAdventure;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class SandFloor : Transform
    {
        static string[] symbolsMap = new string[]
        {
            " ~",
            " ≈",
        };

        public SandFloor(Position position, int w) : base(position, (byte)w)
        {
            type = (byte)VanillaTransforms.sandFloor;
            Initialize();
        }

        public override void SetStaticData()
        {
            DefaultWorldLayer[type] = World.FloorLayerId;
        }

        public override void Collapse() => DropItem(new SandFloorItem());

        public override string GetSymbol() => GetVariation(symbolsMap);

        public override Color GetColor() => new Color(196, 190, 32);
    }
}
