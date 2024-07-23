using System;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Ruine : Transform
    {
        public Ruine(World world, Position position, int worldLayer = -1) : base(world, position)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            renderFieldType = RenderFieldType.ruine;
            this.isObstacle = false;
            Initialize();
        }
    }
}