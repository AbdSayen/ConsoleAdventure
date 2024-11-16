using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Ruine : Transform
    {
        public Ruine(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)RenderFieldType.ruine;
            this.isObstacle = false;

            AddTypeToMap<Ruine>(type);

            Initialize();
        }

        public override void Collapse()
        {
            if(ConsoleAdventure.rand.Next(0, 3) == 0)
            {
                new Loot(position, w, new List<Stack>() { new Stack(new StoneItem(), 1) });
            }
        }

        public override string GetSymbol()
        {
            return "::";
        }

        public override Color GetColor()
        {
            return Color.Gray;
        }
    }
}