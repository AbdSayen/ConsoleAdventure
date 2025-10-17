using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine
{
    public class Basalt : Transform
    {
        public Basalt(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.worldLayer = World.BlocksLayerId;
            type = (int)VanillaTransforms.basalt;
            isObstacle = true;
            hardness = 1.8f;
            burnType = 1;

            AddTypeToMap();

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack>() { new Stack(new BasaltItem(), 1) });
        }
        
        public override string GetSymbol()
        {
            return "‡‡";
        }

        public override Color GetColor()
        {
            return new Color(10, 10, 10);
        }

        public override Color? GetBGColor()
        {
            return new Color(45, 45, 45);
        }
    }
}
