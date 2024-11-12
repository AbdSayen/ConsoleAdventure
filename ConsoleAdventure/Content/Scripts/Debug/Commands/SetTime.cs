using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.Debug.Commands
{
    public class SetTime : Command
    {
        public SetTime() 
        {
            Name = "addtime";
            Description = "set world time";
            Arguments = new List<string>()
            {
                "hours",
                "mins"
            };
        }

        public override void Logic(string[] args, short id = -2)
        {

            int h = GetIntArg(args, "hours");
            int m = GetIntArg(args, "mins");

            try
            {
                ConsoleAdventure.world.time.PassTime((h * 3600) + (m * 60));
            }

            catch (Exception) { }          
        }
    }
}
