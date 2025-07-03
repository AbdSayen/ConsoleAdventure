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
    public class NoCollision : Command
    {
        public NoCollision() 
        {
            Name = "nocollision";
            Description = "set/reset collision";
            Arguments = new List<string>()
            {
            };
        }

        public override void Logic(string[] args, short id = -2)
        {
            ConsoleAdventure.NoCollision = !ConsoleAdventure.NoCollision;
            if (!ConsoleAdventure.NoCollision) Loger.AddLog("collision activated!");
            if (ConsoleAdventure.NoCollision) Loger.AddLog("collision deactivated!");
        }
    }
}
