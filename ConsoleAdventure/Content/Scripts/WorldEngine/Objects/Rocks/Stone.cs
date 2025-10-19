using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class Stone : Transform
    {
        public Stone(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.stone;
            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
            BurnType[type] = 2;
        }

        public override void Collapse() => DropItem(new StoneItem());

        public override string GetSymbol() => "██";

        public override Color GetColor() => Color.Gray;
    }
}