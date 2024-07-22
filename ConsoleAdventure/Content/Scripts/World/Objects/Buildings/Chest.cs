using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine
{
    public class Chest : Storage
    {
        public Chest(World world, Position position, List<Stack> items, int worldLayer = -1) : base(world, position, items)
        {
            renderFieldType = RenderFieldType.chest;
            Initialize();
        }
    }
}
