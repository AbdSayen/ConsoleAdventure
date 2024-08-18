using Microsoft.Xna.Framework;
using System;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Water : Transform
    {
        public Water(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)RenderFieldType.water;
            isObstacle = true;
            hardness = -1;

            AddTypeToMap<Water>(type);

            Initialize();
        }

        public override string GetSymbol()
        {
            return "≈≈";
        }

        public override Color GetColor()
        {
            return new(16, 29, 211);
        }
    }
}