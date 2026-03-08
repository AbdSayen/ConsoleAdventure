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
    public class ShowTimers : Command
    {
        public ShowTimers() 
        {
            Name = "showtimers";
            Description = "on/off display timers";
            Arguments = new List<string>()
            {
            };
        }

        public override void Logic(string[] args, short id = -2)
        {
            Display.IsTimersShowed = !Display.IsTimersShowed;
            if (Display.IsTimersShowed) Loger.AddLog("timers shown!");
            if (!Display.IsTimersShowed) Loger.AddLog("timers hidden!");
        }
    }
}
