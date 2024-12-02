using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class UnloadedTransform : Transform
    {
        public UnloadedTransform(Position position, int w, int worldLayer, int modType) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (byte)modType;
            isObstacle = true;
            hardness = 1f;

            Initialize();
        }

        public override string GetSymbol()
        {
            return " ?";
        }

        public override Color? GetBGColor()
        {
            Color color = new Color(255, 0, 255);

            if (worldLayer == 0) color = new Color(90, 0, 90);

            return color;
        }

        public override Color GetColor()
        {
            return new Color(10, 0, 10);
        }
    }
}
