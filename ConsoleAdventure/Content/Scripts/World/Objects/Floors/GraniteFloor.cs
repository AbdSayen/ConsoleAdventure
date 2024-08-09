using Microsoft.Xna.Framework;
using System;
using System.CodeDom;

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

        //byte Sindex;

        public GraniteFloor(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.FloorLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)RenderFieldType.graniteFloor;
            isObstacle = false;

            AddTypeToMap<GraniteFloor>(type);

            Initialize();

            //Sindex = (byte)ConsoleAdventure.rand.Next(0, symbolsMap.Length);
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