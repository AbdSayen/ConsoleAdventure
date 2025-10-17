using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.Player;
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

            type = (int)VanillaTransforms.climb;

            AddTypeToMap();
            Initialize();
        }

        public override void SetStaticData()
        {
            Hardness[type] = -1;
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
            Player player = world.GetLocalPlayer();

            player.SetPosition(player.position, player.w++);
        }

        public override void OnTheScreen()
        {
            if (w == world.Surface - 1)
                Light.Add(position.x, position.y, w, Light.GetSunLightColor(), 8.5f);
        }
    }
}
