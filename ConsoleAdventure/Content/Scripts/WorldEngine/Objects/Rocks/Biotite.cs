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
    public class Biotite : Transform
    {
        public Biotite(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.biotite;
            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
            Hardness[type] = 0.5f;
        }

        public override void Collapse() => DropItem(new BiotiteItem());

        public override string GetSymbol() => "≡≡";

        public override Color GetColor() => new Color(45, 45, 45);

        public override Color? GetBGColor() => new Color(15, 15, 15);
    }
}
