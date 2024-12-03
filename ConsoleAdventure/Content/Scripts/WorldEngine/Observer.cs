using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Content.Scripts.WorldEngine
{
    public class Observer
    {
        public Position position;
        public int w;

        public Observer(Position pos, int w)
        {
            this.position = pos;
            this.w = w;
        }
    }
}
