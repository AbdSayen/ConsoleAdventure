using ConsoleAdventure.Content.Scripts.Player;
using ConsoleAdventure.WorldEngine;
using System.Collections.Generic;


namespace ConsoleAdventure.Content.Scripts.Debug.Commands
{
    public class PlaceHole : Command
    {
        public PlaceHole() 
        {
            Name = "placehole";
            Description = "place hole";
            Arguments = new List<string>()
            {
            };
        }

        public override void Logic(string[] args, short id = -2)
        {
            Player.Player player = ConsoleAdventure.world.GetLocalPlayer();
            if(player.w > 0)
            {
                new Descent(player.position, player.w);
                new Climb(player.position, player.w - 1);
            }
        }
    }
}
