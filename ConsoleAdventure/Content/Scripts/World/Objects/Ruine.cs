namespace ConsoleAdventure.World
{
    internal class Ruine : Transform
    {
        public Ruine(WorldEngine.World world, Position position, int worldLayer) : base(world, position, worldLayer)
        {
            renderFieldType = WorldEngine.RenderFieldType.ruine;
            isObstacle = false;
        }
    }
}