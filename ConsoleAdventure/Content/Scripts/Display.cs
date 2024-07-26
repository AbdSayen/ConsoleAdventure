using ConsoleAdventure.Settings;
using ConsoleAdventure.WorldEngine;

namespace ConsoleAdventure
{
    internal class Display
    {
        private World world;
        public Display(World world)
        {
            this.world = world;
        }

        public string DisplayInfo()
        {
            return
                $"{Docs.GetInfo()}\n" +
                $"{world.time.GetTime()}\n" +
                $"X:{world.players[0].position.x} Y:{world.players[0].position.y}\n" +
                $"{TextAssets.Structure} {world.GetField(world.players[0].position.x, world.players[0].position.y, World.BlocksLayerId)}\n" +
                (ConsoleAdventure.isPause ? TextAssets.Paused : "") + "\n\n"
                ;
        }

        public void DrawWorld()
        {
            world.Render();
        }

        public string DisplayInventory()
        {
            return
                $"{TextAssets.Inventory}\n" +
                $"{world.players[0].inventory.GetInfo()}\n" +
                $"{Loger.GetLogs()}"
                ;
        }
    }
}
