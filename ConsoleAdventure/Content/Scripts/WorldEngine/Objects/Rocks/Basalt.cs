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
        public Basalt(Position position, int w) : base(position, (byte)w)
        {
            worldLayer = World.BlocksLayerId;
            type = (int)VanillaTransforms.basalt;

            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
            Hardness[type] = 1.8f;
            BurnType[type] = 2;
        }

        public override void Collapse() => DropItem(new BasaltItem());

        public override string GetSymbol() => "‡‡";

        public override Color GetColor() => new Color(10, 10, 10);

        public override Color? GetBGColor() => new Color(45, 45, 45);
    }
}
