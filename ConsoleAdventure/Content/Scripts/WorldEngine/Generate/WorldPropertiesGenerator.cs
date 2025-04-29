using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.WorldEngine.Generate
{
    public class WorldPropertiesGenerator
    {
        public string processHint
        {
            get
            {
                return processHint;
            }
            set
            {
                ConsoleAdventure.progressBar.stepText = value;
            }
        }
        public int processProgress
        {
            get
            {
                return processProgress;
            }
            set
            {
                ConsoleAdventure.progressBar.Progress = (uint)value;
            }
        }

        public async Task UpdateWorldProperties(Tags tags, World world)
        {
            processHint = "Generate something?..";
            processProgress = 0;
        }
    }
}
