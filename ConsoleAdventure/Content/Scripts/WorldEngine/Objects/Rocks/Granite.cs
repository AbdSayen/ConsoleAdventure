using ConsoleAdventure.Content.Scripts;
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
    public class Granite : Transform
    {
        public Granite(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.granite;
            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
            Hardness[type] = 2f;
            BurnType[type] = 2;
        }

        public override void Collapse() => DropItem(new GraniteItem());

        public override string GetSymbol() => "##";

        public override Color GetColor() => new Color(10, 10, 10);

        public override Color? GetBGColor() => new Color(75, 75, 75);
    }
}
