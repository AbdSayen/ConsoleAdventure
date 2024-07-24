using Microsoft.Xna.Framework;
using System;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Water : Transform
    {
        public Water(World world, Position position, int worldLayer = -1) : base(world, position)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            renderFieldType = RenderFieldType.water;
            isObstacle = true;
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