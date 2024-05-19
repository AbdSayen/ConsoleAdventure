namespace ConsoleAdventure.World
{
    public class Wall : Transform
    {
        public Wall(WorldEngine.World world, Position position, int worldLayer) : base(world, position, worldLayer)
        {
            renderFieldType = WorldEngine.RenderFieldType.wall;
            isObstacle = true;
        }
    }
}