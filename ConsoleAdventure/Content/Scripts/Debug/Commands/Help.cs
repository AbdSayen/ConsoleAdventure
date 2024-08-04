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
                if (args.Length < 1)
                {
                    for (int i = 0; i < Commands.Count; i++)
                    {
                        Loger.AddLog(GetCommandHelp(Commands.ElementAt(i).Key));
                    }
                }
                else
                {
                    string name = GetStringArg(args, "command");
                    if (Commands.Keys.Contains(name))
                        Loger.AddLog(GetCommandHelp(name) + "\n  " + Commands[name].Description.Replace("\n", "\n  "));
                }
            }

            catch (Exception) { }          
        }

        private string GetCommandHelp(string cmd)
        {
            Command command = Commands[cmd];
            string argsText = "";

            for (int j = 0; j < command.Arguments.Count; j++)
            {
                argsText += command.Arguments[j];
                if (j < command.Arguments.Count - 1)
                    argsText += ", ";
            }

            return command.Name + " - " + argsText;
        }
    }
}
