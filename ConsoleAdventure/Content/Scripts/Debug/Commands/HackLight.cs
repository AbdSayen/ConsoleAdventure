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
    public class HackLight : Command
    {
        public HackLight() 
        {
            Name = "hacklight";
            Description = "gives a complete initial\nvision";
            Arguments = new List<string>()
            {
            };
        }

        public override void Logic(string[] args, short id = -2)
        {
            Light.hackLight = !Light.hackLight;
            if (Light.hackLight) Loger.AddLog("now you can see in the\ndark!");
            if (!Light.hackLight) Loger.AddLog("you stopped seeing in\nthe dark!");
        }
    }
}
