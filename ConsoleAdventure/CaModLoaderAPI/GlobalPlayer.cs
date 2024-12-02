using ConsoleAdventure.Content.Scripts.Player;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.CaModLoaderAPI
{
    public class GlobalPlayer
    {
        public virtual bool OnTheScreen(Player player)
        {
            return true;
        }

        public virtual bool CraftItem(Player player, Recipe recipe)
        {
            return true;
        }

        public virtual bool? CanBuildAt(Player player, Position pos, int layer)
        {
            return null;
        }

        public virtual bool? CanDestroyAt(Player player, Position pos, int layer)
        {
            return null;
        }
    }
}
