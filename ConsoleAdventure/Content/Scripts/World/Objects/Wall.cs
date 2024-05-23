namespace ConsoleAdventure.World
{
    public class Wall : Transform
    {
        public Wall(WorldEngine.World world, Position position, int worldLayer = -1) : base(world, position)
        {
            if (worldLayer == -1) this.worldLayer = WorldEngine.World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            renderFieldType = WorldEngine.RenderFieldType.wall;
            isObstacle = true;
            Initialize();
        }
    }
}