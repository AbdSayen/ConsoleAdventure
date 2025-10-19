using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class Web : Transform
    {
        public Web(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.web;
            Initialize();
        }

        public override void SetStaticData()
        {
            Hardness[type] = 0.1f;
            BurnType[type] = 1;
        }

        public override void Collapse() => DropItem(new WebItem());

        public override string GetSymbol() => "¼¼";

        public override Color GetColor() => new Color(120, 120, 120);
    }
}