using ConsoleAdventure.Content.Scripts;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Climb : Transform
    {
        public Climb(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)RenderFieldType.climb;
            isObstacle = false;
            hardness = -1;

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
            if (world.GetLocalPlayer().SetPosition(world.GetLocalPlayer().position, 1))
            {
                ConsoleAdventure.curDeep = 1;
            }
        }

        public override void OnTheScreen()
        {
            Light.Add(position.x, position.y, w, Light.GetSunLightColor(), 8.5f);
        }
    }
}
