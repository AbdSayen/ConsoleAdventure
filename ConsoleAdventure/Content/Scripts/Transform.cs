using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using System.Threading;

namespace ConsoleAdventure
{
    abstract public class Transform
    {
        protected WorldEngine.World world;
        protected int worldLayer;
        
        public Position position = new Position(0, 0);
        public RenderFieldType renderFieldType;
        public bool isObstacle;

        protected Transform(WorldEngine.World world, int worldLayer)
        {
            this.world = world;
            this.worldLayer = worldLayer;
        }

        public virtual void Move(int stepSize, Rotation rotation)
        {
            world.MoveSubject(this, worldLayer, stepSize, rotation);
        }
    }
}
