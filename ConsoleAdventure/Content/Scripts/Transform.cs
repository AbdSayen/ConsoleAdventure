using ConsoleAdventure.World;
using System.Collections.Generic;

namespace ConsoleAdventure
{
    public class Transform
    {
        public int x;
        public int y;
        public bool isObstacle;

        public void PostInFields(int layerId, List<List<List<Field>>> fields)
        {
            fields[layerId][y][x].content = this;
        }
    }
}
