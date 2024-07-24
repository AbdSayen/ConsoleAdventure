using Microsoft.Xna.Framework;
using System;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Floor : Transform
    {
        public Floor(World world, Position position, int worldLayer = -1) : base(world, position)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.FloorLayerId;
            else this.worldLayer = worldLayer;

            renderFieldType = RenderFieldType.floor;
            //color = Color.Gray;
            isObstacle = false;
            Initialize();
        }

        public override string GetSymbol()
        {
            return " .";
        }

        public override Color GetColor()
        {
            return Color.Gray;
        }
    }
}