using CaModLoaderAPI;
using ConsoleAdventure;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class AlfisolFloor : Transform
    {
        static string[] symbolsMap = new string[]
        {
            " ~",
            " ≈",
            " .",
            " ,",
            " `",
        };

        public AlfisolFloor(Position position, int w) : base(position, (byte)w)
        {
            type = (byte)VanillaTransforms.alfisolFloor;
            Initialize();
        }

        public override void SetStaticData()
        {
            DefaultWorldLayer[type] = World.FloorLayerId;
        }

        public override void Collapse() => DropItem(new AlfisolFloorItem());

        public override string GetSymbol() => GetVariation(symbolsMap);

        public override Color GetColor() => new Color(54, 43, 32);
    }
}
