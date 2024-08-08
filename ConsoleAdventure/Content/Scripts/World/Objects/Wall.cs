using Microsoft.Xna.Framework;
using System;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Wall : Transform
    {
        public Wall(Position position, int w, int worldLayer = -1) : base(position, w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            type = (int)RenderFieldType.wall;
            isObstacle = true;

            AddTypeToMap<Wall>(type);

            Initialize();
        }

        public override string GetSymbol()
        {
            return "##";
        }

        public override Color GetColor()
        {
            return Color.White;
        }
    }
}