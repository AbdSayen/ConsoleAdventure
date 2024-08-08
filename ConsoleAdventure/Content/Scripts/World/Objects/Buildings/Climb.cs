using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Climb : Transform
    {
        public Climb(Position position, int w, int worldLayer = 1) : base(position, w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            type = (int)RenderFieldType.climb;
            isObstacle = false;

            AddTypeToMap<Climb>(type);

            Initialize();
        }

        public override string GetSymbol()
        {
            return "▲▲";
        }

        public override Color GetColor()
        {
            return Color.Gray;
        }

        public override void Interaction()
        {
            if (world.players[0].SetPosition(world.players[0].position, 1))
                ConsoleAdventure.curDeep = 1;
        }  
    }
}
