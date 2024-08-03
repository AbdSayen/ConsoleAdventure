using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.Debug.Commands
{
    public class Help : Command
    {
        public Help() 
        {
            Name = "help";
            Description = "help for commands";
            Arguments = new List<string>()
            {
                "command"
            };
        }

        public override void Logic(string[] args)
        {
            try
            {
                for (int i = 0; i < Commands.Count; i++)
                {
                    Command command = Commands[i];
                    string argsText = ""; 

                    for (int j = 0; j < command.Arguments.Count; j++)
                    {
                        argsText += " " + command.Arguments[j];
                    }

                    Loger.AddLog(command.Name + argsText);
                }
            }

            catch (Exception) { }          
        }
    }
}
