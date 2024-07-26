using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Tree : Transform
    {
        public Tree(Position position, int worldLayer = -1) : base(position)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            type = (int)RenderFieldType.tree;
            isObstacle = true;

            AddTypeToMap<Tree>(type);

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, new List<Stack> { new Stack(new Log(), 3), new Stack(new Apple(), 1) });
        }

        public override string GetSymbol()
        {
            return " *";
        }

        public override Color GetColor()
        {
            return new(13, 152, 20);
        }
    }
}