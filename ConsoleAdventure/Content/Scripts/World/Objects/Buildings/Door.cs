using Microsoft.Xna.Framework;
using System;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Door : Transform
    {
        public Door(Position position, int w, int worldLayer = 1) : base(position, w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            type = (int)RenderFieldType.door;
            isObstacle = false;

            AddTypeToMap<Door>(type);

            Initialize();
        }

        public override string GetSymbol()
        {
            return "[]";
        }

        public override Color GetColor()
        {
            return new(94, 61, 38);
        }
    }
}