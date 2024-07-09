using Microsoft.Xna.Framework;

namespace ConsoleAdventure.WorldEngine
{
    public class Floor : Transform
    {
        public Floor(World world, Position position = null, int worldLayer = -1) : base(world, position)
        {
            if (position != null) this.position = position;
            else this.position = new Position(0, 0);
            if (worldLayer == -1) this.worldLayer = World.FloorLayerId;
            else this.worldLayer = worldLayer;

            renderFieldType = RenderFieldType.floor;
            //color = Color.Gray;
            isObstacle = false;
            Initialize();
        }
    }
}