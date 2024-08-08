using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Descent : Transform
    {
        public Descent(Position position, int w, int worldLayer = 1) : base(position, w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            type = (int)RenderFieldType.descent;
            isObstacle = false;

            AddTypeToMap<Descent>(type);

            Initialize();
        }

        public override string GetSymbol()
        {
            return "▼▼";
        }

        public override Color GetColor()
        {
            return Color.Gray;
        }
    }
}