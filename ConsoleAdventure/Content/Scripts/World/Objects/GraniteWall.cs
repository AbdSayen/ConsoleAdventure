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
    public class GraniteWall : Transform
    {
        public GraniteWall(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)RenderFieldType.graniteWall;
            isObstacle = true;
            hardness = 2;

            AddTypeToMap<GraniteWall>(type);

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack>() { new Stack(new GraniteWallItem(), 1) });
        }
        
        public override string GetSymbol()
        {
            return "∫∫";
        }

        public override Color GetColor()
        {
            return new Color(45, 45, 45);
        }

        public override Color? GetBGColor()
        {
            return base.GetBGColor();
        }
    }
}
