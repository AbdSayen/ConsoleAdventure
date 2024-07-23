using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class Plank : Transform
    {
        public Plank(World world, Position position, int worldLayer = -1) : base(world, position)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            renderFieldType = RenderFieldType.log;
            isObstacle = true;
            Initialize();
        }

        public override void Collapse()
        {
            new Loot(world, position, new List<Stack>() { new Stack(new Log(), 1) });
        }
    }
}