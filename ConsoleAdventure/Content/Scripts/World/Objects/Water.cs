using Microsoft.Xna.Framework;

namespace ConsoleAdventure.WorldEngine
{
    public class Water : Transform
    {
        public Water(World world, Position position, int worldLayer = -1) : base(world, position)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            renderFieldType = RenderFieldType.water;
            isObstacle = true;
            Initialize();
        }
    }
}