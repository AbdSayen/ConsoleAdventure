using Microsoft.Xna.Framework;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class ObsidianFloor : Transform
    {
        static string[] symbolsMap = new string[]
        {
            " /",
            " ¬",
            " Ϟ",
        };

        public ObsidianFloor(Position position, int w) : base(position, (byte)w)
        {
            worldLayer = World.FloorLayerId;
            type = (int)VanillaTransforms.obsidianFloor;

            Initialize();
        }

        public override void Collapse() => DropItem(new ObsidianFloorItem());

        public override string GetSymbol() => GetVariation(symbolsMap);

        public override Color GetColor() => new Color(25, 0, 84);
    }
}