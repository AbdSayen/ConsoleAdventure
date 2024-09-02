using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class BrokenLog : Transform
    {
        public BrokenLog(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)RenderFieldType.brokenLog;
            this.isObstacle = false;

            AddTypeToMap<BrokenLog>(type);

            Initialize();
        }

        public override void Collapse()
        {
            if(ConsoleAdventure.rand.Next(0, 3) == 0)
            {
                new Loot(position, w, new List<Stack>() { new Stack(new Log(), 1) });
            }
        }

        public override string GetSymbol()
        {
            return "⸗₋";
        }

        public override Color GetColor()
        {
            return new(74, 41, 18);
        }
    }
}