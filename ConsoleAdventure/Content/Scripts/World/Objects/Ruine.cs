namespace ConsoleAdventure.World
{
    public class Ruine : Transform
    {
        public Ruine(WorldEngine.World world, Position position = null, int worldLayer = -1) : base(world, position)
        {
            if (position != null) this.position = position;
            else this.position = new Position(0, 0);
            if (worldLayer == -1) this.worldLayer = WorldEngine.World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            renderFieldType = WorldEngine.RenderFieldType.ruine;
            this.isObstacle = false;
            Initialize();
        }
    }
}