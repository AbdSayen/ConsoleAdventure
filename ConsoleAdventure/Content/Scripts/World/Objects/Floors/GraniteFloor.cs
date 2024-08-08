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

        byte Sindex;

        public GraniteFloor(Position position, int w, int worldLayer = -1) : base(position, w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.FloorLayerId;
            else this.worldLayer = worldLayer;

            type = (int)RenderFieldType.graniteFloor;
            isObstacle = false;

            AddTypeToMap<GraniteFloor>(type);

            Initialize();

            Sindex = (byte)ConsoleAdventure.rand.Next(0, symbolsMap.Length);
        }

        public override string GetSymbol()
        {
            return symbolsMap[Sindex];
        }

        public override Color GetColor()
        {
            return new Color(35, 35, 35);
        }
    }
}