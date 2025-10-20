using CaModLoaderAPI;
using ConsoleAdventure;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class BlackSoilFloor : Transform
    {
        static string[] symbolsMap = new string[]
        {
            " ~",
            " ≈",
            " .",
            " ,",
            " `",
        };

        public BlackSoilFloor(Position position, int w) : base(position, (byte)w)
        {
            type = (byte)VanillaTransforms.blackSoilFloor;
            Initialize();
        }

        public override void SetStaticData()
        {
            DefaultWorldLayer[type] = World.FloorLayerId;
        }

        public override void Collapse() => DropItem(new BlackSoilFloorItem());

        public override string GetSymbol() => GetVariation(symbolsMap);

        public override Color GetColor() => new Color(20, 20, 20);
    }
}
