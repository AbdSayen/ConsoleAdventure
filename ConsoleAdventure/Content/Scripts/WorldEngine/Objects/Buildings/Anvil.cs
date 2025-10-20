using Microsoft.Xna.Framework;
using System;
using System.Collections;

namespace ConsoleAdventure.WorldEngine
{
    public class Anvil : Transform
    {
        public Anvil(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.anvil;
            Initialize();
        }

        public override void Collapse() => DropItem(new AnvilItem());

        public override string GetSymbol() => " σ";

        public override Color GetColor() => Color.Gray;
    }
}