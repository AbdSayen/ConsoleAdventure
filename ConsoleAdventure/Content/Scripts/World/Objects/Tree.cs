namespace ConsoleAdventure.World
{
    internal class Tree : Transform
    {
        public Tree(WorldEngine.World world, Position position, int worldLayer) : base(world, position, worldLayer)
        {
            renderFieldType = WorldEngine.RenderFieldType.tree;
            isObstacle = true;
        }
    }
}