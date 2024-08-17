using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.Debug.Commands
{
    public class TeleportPlayer : Command
    {
        public TeleportPlayer() 
        {
            Name = "tp";
            Description = "teleports the player \nto specified coordinates";
            Arguments = new List<string>()
            {
                "x",
                "y"
            };
        }

        public override void Logic(string[] args, short id = -1)
        {

            int x = GetIntArg(args, "x");
            int y = GetIntArg(args, "y");

            try
            {
                if (x < 0)  
                    return;
                if (y < 0) 
                    return;
                if (x > ConsoleAdventure.world.size) 
                    return;
                if (y > ConsoleAdventure.world.size) 
                    return;

                if (id == -1) id = 0;
                Player.Player pl = ConsoleAdventure.world.players[id];
                pl.SetPosition(new(x, y));
            }

            catch (Exception) { }          
        }
    }
}
