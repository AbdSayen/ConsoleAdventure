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
    public class Granulite : Transform
    {
        public Granulite(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)VanillaTransforms.granulite;

            AddTypeToMap();

            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
            Hardness[type] = 2f;
            BurnType[type] = 2;
        }
        public override void Collapse()
        {
            new Loot(position, w, new List<Stack>() { new Stack(new GranuliteItem(), 1) });
        }
        
        public override string GetSymbol()
        {
            return "≤≤";
        }

        public override Color GetColor()
        {
            return new Color(64, 19, 19);
        }

        public override Color? GetBGColor()
        {
            return new Color(100, 100, 73);
        }
    }
}
