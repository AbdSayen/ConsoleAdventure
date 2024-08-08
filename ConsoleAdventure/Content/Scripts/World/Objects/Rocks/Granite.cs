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
    public class Granite : Transform
    {
        public Granite(Position position, int w, int worldLayer = -1) : base(position, w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            type = (int)RenderFieldType.granite;
            isObstacle = true;

            
            AddTypeToMap<Granite>(type);

            Initialize();
        }

        public override string GetSymbol()
        {
            return "##";
        }

        public override Color GetColor()
        {
            return new Color(45, 45, 45);
        }

        public override Color? GetBGColor()
        {
            return Color.Gray;
        }
    }
}
