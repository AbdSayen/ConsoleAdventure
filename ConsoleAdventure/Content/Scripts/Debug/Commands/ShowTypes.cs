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
    public class ShowTypes : Command
    {
        public ShowTypes() 
        {
            Name = "showtypes";
            Description = "on/off display types";
            Arguments = new List<string>()
            {
            };
        }

        public override void Logic(string[] args, short id = -2)
        {
            ConsoleAdventure.ShowTypes = !ConsoleAdventure.ShowTypes;
            if (ConsoleAdventure.ShowTypes) Loger.AddLog("types is shown!");
            if (!ConsoleAdventure.ShowTypes) Loger.AddLog("types is hidden!");
        }
    }
}
