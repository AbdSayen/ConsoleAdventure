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
    public class Zoisite : Transform
    {
        public Zoisite(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)VanillaTransforms.zoisite;
            isObstacle = true;
            hardness = 2;

            AddTypeToMap<Zoisite>(type);

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack>() { new Stack(new ZoisiteItem(), 1) });
        }
        
        public override string GetSymbol()
        {
            return "ηη";
        }

        public override Color GetColor()
        {
            return new Color(6, 61, 31);
        }

        public override Color? GetBGColor()
        {
            return new Color(19, 124, 50);
        }
    }
}
