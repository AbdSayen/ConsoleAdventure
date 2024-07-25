using Microsoft.VisualBasic.Logging;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class Plank : Transform
    {
        public Plank(World world, Position position, int worldLayer = -1) : base(world, position)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            type = (int)RenderFieldType.log;
            isObstacle = true;

            AddTypeToMap<Plank>(type);

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(world, position, new List<Stack>() { new Stack(new Log(), 1) });
        }

        public override string GetSymbol()
        {
            return "≡≡";
        }

        public override Color GetColor()
        {
            return new(94, 61, 38);
        }
    }
}