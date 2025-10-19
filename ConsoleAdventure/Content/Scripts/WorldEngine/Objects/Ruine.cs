using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure.WorldEngine
{
    public class Ruine : Transform
    {
        public Ruine(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.ruine;
            Initialize();
        }

        public override void Collapse()
        {
            if(ConsoleAdventure.rand.Next(0, 3) == 0)
            {
                new Loot(position, w, new List<Stack>() { new Stack(new StoneItem(), 1) });
            }
        }

        public override string GetSymbol() => "::";

        public override Color GetColor() => Color.Gray;
    }
}