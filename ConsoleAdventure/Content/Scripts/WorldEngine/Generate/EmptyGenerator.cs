using ConsoleAdventure.WorldEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine.Generate
{
    public class EmptyGenerator
    {
        public World world;

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

        public virtual async Task Generate(World world)
        {
            this.world = world;

            processHint = "Generate something?..";
            processProgress = 0;
        }
    }
}
