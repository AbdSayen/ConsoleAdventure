using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine
{
    public class WorldLevel
    {
        public bool HasSun { get; set; } = false;
        public Color SunLight { get; set; } = Color.Black;
        public Color NightLight { get; set;} = Color.Black;
    }
}
