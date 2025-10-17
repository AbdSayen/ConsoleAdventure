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

        public SandFloor(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            this.worldLayer = World.FloorLayerId;

            type = (byte)VanillaTransforms.sandFloor;

            AddTypeToMap();
            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack> { new Stack(new SandFloorItem(), 1) });
        }

        public override string GetSymbol()
        {
            return symbolsMap[Utils.HashNoise(position.x, position.y, 2)];
        }

        public override Color GetColor()
        {
            return new Color(196, 190, 32);
        }
    }
}
