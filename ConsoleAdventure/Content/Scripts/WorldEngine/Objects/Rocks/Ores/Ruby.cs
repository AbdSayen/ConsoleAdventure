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
    public class Ruby : Transform
    {
        public Ruby(Position position, int w) : base(position, (byte)w)
        {
            worldLayer = World.BlocksLayerId;
            type = (int)VanillaTransforms.ruby;

            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
            Hardness[type] = 4f;
        }

        public override void Collapse() => DropItem(new RubyItem());

        public override string GetSymbol() => "♦♦";

        public override Color GetColor() => new Color(200, 21, 110);

        public override Color? GetBGColor() => new Color(19, 124, 50);
    }
}
