using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class Plank : Transform
    {
        public Plank(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)VanillaTransforms.log;
            isObstacle = true;
            burnType = 0;
            hardness = 0.8f;

            AddTypeToMap<Plank>(type);

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack>() { new Stack(new Log(), 1) });
        }

        public override string GetSymbol()
        {
            return "≡≡";
        }

        public override Color GetColor()
        {
            return new(94, 61, 38);
        }

        public override void AfterBurning()
        {
            if (ConsoleAdventure.rand.Next(0, 2) == 1)
            {
                new Charcoal(position, w);
            }

            else
            {
                base.AfterBurning();
            }
        }
    }
}