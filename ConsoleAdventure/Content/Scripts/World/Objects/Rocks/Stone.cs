using Microsoft.Xna.Framework;
using System;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Stone : Transform
    {
        public Stone(Position position, int w, int worldLayer = -1) : base(position, w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            type = (int)RenderFieldType.stone;
            isObstacle = true;

            AddTypeToMap<Stone>(type);

            Initialize();
        }

        public override string GetSymbol()
        {
            return "██";
        }

        public override Color GetColor()
        {
            return Color.Gray;
        }
    }
}