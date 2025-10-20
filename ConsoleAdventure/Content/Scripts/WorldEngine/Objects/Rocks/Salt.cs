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
        public Salt(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.salt;
            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
            Hardness[type] = 1f;
        }

        public override void Collapse() => DropItem(new SaltItem());

        public override string GetSymbol() => "∆∆";

        public override Color GetColor() => new Color(193, 157, 175);
    }
}
