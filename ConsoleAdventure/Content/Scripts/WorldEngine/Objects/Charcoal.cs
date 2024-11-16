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

            type = (int)RenderFieldType.charcoal;
            isObstacle = true;
            hardness = 0.6f;

            AddTypeToMap<Charcoal>(type);

            Initialize();
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