using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine.Levels
{
    public class Surface : WorldLevel
    {
        public Surface()
        {
            HasSun = true;
            SunLight = Color.White;
            NightLight = new(25, 25, 25);
        }
    }
}
