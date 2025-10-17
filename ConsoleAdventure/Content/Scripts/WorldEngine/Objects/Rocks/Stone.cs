using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Stone : Transform
    {
        public Stone(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)VanillaTransforms.stone;
            isObstacle = true;
            burnType = 1;

            AddTypeToMap();

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack>() { new Stack(new StoneItem(), 1) });
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