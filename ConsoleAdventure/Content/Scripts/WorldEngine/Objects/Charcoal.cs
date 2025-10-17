using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class Charcoal : Transform
    {
        public Charcoal(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)VanillaTransforms.charcoal;

            AddTypeToMap();

            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
            Hardness[type] = 0.6f;
        }

        public override void Collapse()
        {
            //new Loot(position, w, new List<Stack>() { new Stack(new Log(), 1) });
        }

        public override string GetSymbol()
        {
            return "≡≡";
        }

        public override Color GetColor()
        {
            return new(45, 45, 45);
        }
    }
}