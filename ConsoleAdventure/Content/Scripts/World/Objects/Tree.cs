using ConsoleAdventure.Settings;

namespace ConsoleAdventure.World
{
    internal class Tree : Transform
    {
        public Tree(WorldEngine.World world, int worldLayer) : base(world, worldLayer)
        {
            renderFieldType = WorldEngine.RenderFieldType.tree;
            isObstacle = true;
            //for (int i = 0; i < 30; i++)
            //{
            //    Dance();
            //}
            //Loger.AddLog("Done.");
        }

        private void Dance()
        {
            Move(1, Rotation.down);
        }
    }
}