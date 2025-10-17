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

            type = (int)VanillaTransforms.water;

            AddTypeToMap();

            Initialize();
        }

        public override void SetStaticData()
        {
            //IsObstacle[type] = true;
            Hardness[type] = -1f;
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