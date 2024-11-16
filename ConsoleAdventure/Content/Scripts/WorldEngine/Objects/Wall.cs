using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Wall : Transform
    {
        public Wall(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)RenderFieldType.wall;
            isObstacle = true;

            AddTypeToMap<Wall>(type);

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack>() { new Stack(new WallItem(), 1) });
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