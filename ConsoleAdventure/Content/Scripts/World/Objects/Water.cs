using Microsoft.Xna.Framework;
using System;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Water : Transform
    {
        public Water(Position position, int worldLayer = -1) : base(position)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            type = (int)RenderFieldType.water;
            isObstacle = true;

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