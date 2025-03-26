using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.Debug.Commands
{
    public class ChunkCommand : Command
    {
        public ChunkCommand() 
        {
            Name = "chunk";
            Description = "???";
            Arguments = new List<string>()
            {
                "attribute",
                "x",
                "y"
            };
        }

        public override void Logic(string[] args, short id = -2)
        {
            string a = GetStringArg(args, "attribute");
            int x = GetIntArg(args, "x");
            int y = GetIntArg(args, "y");

            try
            {
                if (a == "load")
                {
                    ConsoleAdventure.world.LoadChunk(x, y, false);
                }

                if (a == "unload")
                {
                    ConsoleAdventure.world.UnloadChunk(x, y);
                }
            }

            catch (Exception) 
            {
                if(a == "a")
                {

                }
            }          
        }
    }
}
