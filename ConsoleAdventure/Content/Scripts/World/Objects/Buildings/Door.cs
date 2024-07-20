using Microsoft.Xna.Framework;
using System;
using ConsoleAdventure.Content.Scripts.Abstracts;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Door : Transform
    {
        public Door(World world, Position position, int worldLayer = 1) : base(world, position)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            renderFieldType = RenderFieldType.door;
            //color = Color.Brown;
            isObstacle = false;
            Initialize();
        }
    }
}