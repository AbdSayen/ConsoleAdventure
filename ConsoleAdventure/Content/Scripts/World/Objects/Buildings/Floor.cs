using Microsoft.Xna.Framework;
using System;
using System.CodeDom;

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

            type = (int)RenderFieldType.floor;
            isObstacle = false;

            AddTypeToMap<Floor>(type);

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