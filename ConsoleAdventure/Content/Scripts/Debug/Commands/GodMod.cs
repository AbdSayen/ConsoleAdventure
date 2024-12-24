using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using SharpDX.DirectWrite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.Debug.Commands
{
    public class GodMode : Command
    {
        public GodMode() 
        {
            Name = "godmode";
            Description = "set/reset god mode";
            Arguments = new List<string>()
            {
            };
        }

        public override void Logic(string[] args, short id = -2)
        {
            ConsoleAdventure.GodMode = !ConsoleAdventure.GodMode;
            if (ConsoleAdventure.GodMode) Loger.AddLog("god mode activated!");
            if (!ConsoleAdventure.GodMode) Loger.AddLog("god mode deactivated!");
        }
    }
}
