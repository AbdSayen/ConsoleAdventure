using ConsoleAdventure.Content.Scripts.Player;
using Microsoft.Xna.Framework;
using System;
using System.Diagnostics;

namespace ConsoleAdventure.WorldEngine
{
    public class Descent : Transform
    {
        public Descent(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.descent;
            Initialize();
        }

        public override void SetStaticData()
        {
            Hardness[type] = -1;
        }

        public override string GetSymbol() => "▼▼";

        public override Color GetColor() => Color.Gray;
        
        public override void Interaction()
        {
            Player player = world.GetLocalPlayer();

            player.SetPosition(player.position, player.w--);
        }
    }
}
