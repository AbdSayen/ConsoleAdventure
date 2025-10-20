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
    public class Zoisite : Transform
    {
        public Zoisite(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.zoisite;
            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
            Hardness[type] = 2f;
        }

        public override void Collapse() => DropItem(new ZoisiteItem());

        public override string GetSymbol() => "ηη";

        public override Color GetColor() => new Color(6, 61, 31);

        public override Color? GetBGColor() => new Color(19, 124, 50);
    }
}
