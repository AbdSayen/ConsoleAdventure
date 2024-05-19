using ConsoleAdventure.WorldEngine;

namespace ConsoleAdventure
{
    abstract public class Transform
    {
        protected WorldEngine.World world;
        protected int worldLayer;
        
        public Position position = new Position(0, 0);
        public RenderFieldType renderFieldType;
        public bool isObstacle;

        protected Transform(WorldEngine.World world, Position position, int worldLayer)
        {
            this.world = world;
            this.worldLayer = worldLayer;
            this.position = position;
        }

        public virtual void Move(int stepSize, Rotation rotation)
        {
            world.MoveSubject(this, worldLayer, stepSize, rotation);
        }
    }
}
