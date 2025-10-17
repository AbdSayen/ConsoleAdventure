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
    public class Salt : Transform
    {
        public Salt(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)VanillaTransforms.salt;
            isObstacle = true;
            hardness = 2;

            AddTypeToMap();

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack>() { new Stack(new SaltItem(), 1) });
        }
        
        public override string GetSymbol()
        {
            return "∆∆";
        }

        public override Color GetColor()
        {
            return new Color(193, 157, 175);
        }
    }
}
