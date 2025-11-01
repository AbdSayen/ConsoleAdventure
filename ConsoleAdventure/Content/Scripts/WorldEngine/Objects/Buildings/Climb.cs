using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.Player;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace ConsoleAdventure.WorldEngine
{
    public class Climb : Transform
    {
        public Climb(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.climb;
            Initialize();
        }

        public override void SetStaticData()
        {
            Hardness[type] = -1;
        }

        public override string GetSymbol() => "▲▲";

        public override Color GetColor() => Color.Gray;

        public override void Interaction()
        {
            Player player = world.GetLocalPlayer();

            player.SetPosition(player.position, player.w++);
        }

        public override void OnTheScreen()
        {
            if (w < world.levels.Levels.Count - 1) 
            {
                WorldLevel level = world.levels.Levels[w + 1];

                if (level.HasSun)
                {
                    Light.Add(position.x, position.y, w, Light.GetSunLightColor(level.SunLight, level.NightLight), 8.5f);
                } 
            }
        }
    }
}
