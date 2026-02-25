using Microsoft.Xna.Framework;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class Floor : Transform
    {
        public Floor(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.floor;
            Initialize();
        }

        public override void SetStaticData()
        {
            DefaultWorldLayer[type] = World.FloorLayerId;
        }

        public override void Collapse() => DropItem(new FloorItem(), 1, material);

        public override string GetSymbol() => GetMaterialSymbol(" .");

        public override Color GetColor() => GetMaterialColor();
    }
}