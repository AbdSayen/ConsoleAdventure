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
                $"Structure: {world.fields[WorldEngine.World.BlocksLayerId][world.players[0].position.y][world.players[0].position.x].structureName}\n\n"
                ;
        }

        public string DisplayBlocksLayer()
        {
            return
                $"{world.Render(World.BlocksLayerId)}\n"
                ;
        }

        public string DisplayMobsLayer()
        {
            return
                $"{world.Render(World.MobsLayerId)}\n"
                ;
        }

        public string DisplayItemsLayer()
        {
            return
                $"{world.Render(World.ItemsLayerId)}\n"
                ;
        }

        public string DisplayFloorLayer()
        {
            return
                $"{world.Render(World.FloorLayerId)}\n"
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
