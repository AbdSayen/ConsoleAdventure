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
    public class Obsidian : Transform
    {
        static string[] symbolsMap = new string[]
        {
            "/ ",
            " /",
            "\\ ",
            " \\",
            "Ϟ ",
            " Ϟ",
        };

        public Obsidian(Position position, int w) : base(position, (byte)w)
        {
            worldLayer = World.BlocksLayerId;
            type = (int)VanillaTransforms.obsidian;

            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
            Hardness[type] = 1.1f;
            BurnType[type] = 2;
        }

        public override void Collapse() => DropItem(new ObsidianItem());

        public override string GetSymbol() => GetVariation(symbolsMap);

        public override Color GetColor() => new Color(0, 0, 0);

        public override Color? GetBGColor() => new Color(25, 0, 84);
    }
}
