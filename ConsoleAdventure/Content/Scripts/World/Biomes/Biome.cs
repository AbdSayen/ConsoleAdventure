using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine
{
    public abstract class Biome
    {
        public List<short> transformTypes = new();
        public int minCountForCreate;

        protected Biome()
        {
        }
    }
}
