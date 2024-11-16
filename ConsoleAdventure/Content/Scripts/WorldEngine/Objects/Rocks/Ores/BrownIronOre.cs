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
    public class BrownIronOre : Transform
    {
        public BrownIronOre(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)RenderFieldType.brownIronOre;
            isObstacle = true;
            hardness = 0.8f;
            burnType = 1;

            AddTypeToMap<BrownIronOre>(type);

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack>() { new Stack(new BrownIronOreItem(), 1) });
        }

        public override string GetSymbol()
        {
            return "§§";
        }

        public override Color GetColor()
        {
            return new Color(206, 83, 33);
        }

        public override Color? GetBGColor()
        {
            return Color.Gray;
        }
    }
}
