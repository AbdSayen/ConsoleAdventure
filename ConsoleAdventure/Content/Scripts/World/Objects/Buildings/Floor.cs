namespace ConsoleAdventure.World
{
    public class Floor : Transform
    {
        public Floor(WorldEngine.World world, Position position = null, int worldLayer = -1) : base(world, position)
        {
            if (position != null) this.position = position;
            else this.position = new Position(0, 0);
            if (worldLayer == -1) this.worldLayer = WorldEngine.World.FloorLayerId;
            else this.worldLayer = worldLayer;

            renderFieldType = WorldEngine.RenderFieldType.floor;
            isObstacle = false;
            Initialize();
        }
    }
}