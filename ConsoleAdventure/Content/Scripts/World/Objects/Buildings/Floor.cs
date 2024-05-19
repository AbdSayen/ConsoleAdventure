namespace ConsoleAdventure.World
{
    internal class Floor : Transform
    {
        public Floor(WorldEngine.World world, Position position, int worldLayer) : base(world, position, worldLayer)
        {
            renderFieldType = WorldEngine.RenderFieldType.floor;
            isObstacle = false;
        }
    }
}