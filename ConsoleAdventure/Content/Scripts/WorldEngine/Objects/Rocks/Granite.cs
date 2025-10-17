using ConsoleAdventure.Content.Scripts;
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
        public Granite(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)VanillaTransforms.granite;

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
            new Loot(position, w, new List<Stack>() { new Stack(new GraniteItem(), 1) });
        }

        public override string GetSymbol()
        {
            return "##";
        }

        public override Color GetColor()
        {
            return new Color(10, 10, 10);
        }

        public override Color? GetBGColor()
        {
            return new Color(75, 75, 75);
        }
    }
}
