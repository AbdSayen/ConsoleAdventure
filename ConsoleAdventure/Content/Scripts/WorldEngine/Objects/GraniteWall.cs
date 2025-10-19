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
    public class GraniteWall : Transform
    {
        public GraniteWall(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.graniteWall;
            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
            Hardness[type] = 2f;
        }

        public override void Collapse() => DropItem(new GraniteWallItem());

        public override string GetSymbol() => "∫∫";

        public override Color GetColor() => new Color(45, 45, 45);
    }
}
