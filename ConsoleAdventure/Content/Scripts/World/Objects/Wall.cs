namespace ConsoleAdventure.Content.Scripts.World
{
    public class Wall : Transform
    {
        public Wall(WorldEngine.World world, int worldLayer) : base(world, worldLayer)
        {
            renderFieldType = WorldEngine.RenderFieldType.wall;
            isObstacle = true;
        }
    }
}