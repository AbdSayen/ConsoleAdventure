using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure
{
    internal class Display
    {
        private WorldEngine.World world;
        public Display(WorldEngine.World world)
        {
            this.world = world;
        }

        public string DisplayInfo()
        {
            return 
                $"{Docs.GetInfo()}\n" +
                $"{world.time.GetTime()}\n"
                ;
        }

        public string DisplayGame()
        {
            return
                $"X:{world.players[0].position.x} Y:{world.players[0].position.y}\n" +
                $"Structure: {world.fields[WorldEngine.World.BlocksLayerId][world.players[0].position.y][world.players[0].position.x].structureName}\n\n" +
                $"{world.Render()}\n"
                ;
        }

        public string DisplayInventory()
        {
            return
                $"Inventory:\n" +
                $"{world.players[0].inventory.GetInfo()}\n" +
                $"{Loger.GetLogs()}"
                ;
        }
    }
}
