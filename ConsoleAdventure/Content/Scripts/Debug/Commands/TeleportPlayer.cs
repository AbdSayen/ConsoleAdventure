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

        public override void Logic(string[] args)
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

                ConsoleAdventure.world.players[0].SetPosition(new(x, y));
            }

            catch (Exception) { }          
        }
    }
}
