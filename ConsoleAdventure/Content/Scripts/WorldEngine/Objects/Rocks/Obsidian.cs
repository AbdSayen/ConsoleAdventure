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

        public Obsidian(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)VanillaTransforms.obsidian;
            isObstacle = true;
            hardness = 1.5f;
            burnType = 1;

            AddTypeToMap<Obsidian>(type);

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack>() { new Stack(new ObsidianItem(), 1) });
        }
        
        public override string GetSymbol()
        {
            return symbolsMap[Utils.HashNoise(position.x, position.y, 6)];
        }

        public override Color GetColor()
        {
            return new Color(0, 0, 0);
        }

        public override Color? GetBGColor()
        {
            return new Color(25, 0, 84);
        }
    }
}
