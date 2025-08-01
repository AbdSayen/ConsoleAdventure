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
    public class Biotite : Transform
    {
        public Biotite(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)VanillaTransforms.biotite;
            isObstacle = true;
            hardness = 0.5f;

            AddTypeToMap(GetType(), type);

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack>() { new Stack(new BiotiteItem(), 1) });
        }
        
        public override string GetSymbol()
        {
            return "≡≡";
        }

        public override Color GetColor()
        {
            return new Color(45, 45, 45);
        }

        public override Color? GetBGColor()
        {
            return new Color(15, 15, 15);
        }
    }
}
