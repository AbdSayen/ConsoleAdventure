using Microsoft.VisualBasic.Logging;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Tree : Transform
    {
        public Tree(World world, Position position = null, int worldLayer = -1) : base(world, position)
        {
            if (position != null) this.position = position;
            else this.position = new Position(0, 0);
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            renderFieldType = RenderFieldType.tree;
            isObstacle = true;
            Initialize();
        }

        public override void Collapse()
        {
            new Loot(world, position, new List<Stack> { new Stack(new Log(), 3), new Stack(new Apple(), 1) });
        }
    }
}