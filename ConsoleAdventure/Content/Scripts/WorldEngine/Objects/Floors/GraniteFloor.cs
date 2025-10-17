using Microsoft.Xna.Framework;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class GraniteFloor : Transform
    {
        static string[] symbolsMap = new string[]
        {
            " .",
            " ~",
            " ,",
        };

        public GraniteFloor(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.FloorLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)VanillaTransforms.graniteFloor;

            AddTypeToMap();

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack> { new Stack(new GraniteFloorItem(), 1) });
        }

        public override string GetSymbol()
        {
            return symbolsMap[Utils.HashNoise(position.x, position.y, 3)];
        }

        public override Color GetColor()
        {
            return new Color(35, 35, 35);
        }
    }
}