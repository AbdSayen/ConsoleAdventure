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
    public class Ruby : Transform
    {
        public Ruby(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)VanillaTransforms.ruby;
            isObstacle = true;
            hardness = 4f;

            AddTypeToMap<Ruby>(type);

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack>() { new Stack(new RubyItem(), 1) });
        }

        public override string GetSymbol()
        {
            return "♦♦";
        }

        public override Color GetColor()
        {
            return new Color(200, 21, 110);
        }

        public override Color? GetBGColor()
        {
            return new Color(19, 124, 50);
        }
    }
}
