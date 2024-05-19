namespace ConsoleAdventure.World
{
    internal class Door : Transform
    {
        public Door(WorldEngine.World world, Position position, int worldLayer) : base(world, position, worldLayer)
        {
            renderFieldType = WorldEngine.RenderFieldType.door;
            isObstacle = false;
        }
    }
}