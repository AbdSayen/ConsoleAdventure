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
                $"X:{world.GetLocalPlayer().position.x} Y:{world.GetLocalPlayer().position.y}\n" +
                (ConsoleAdventure.isPause ? TextAssets.Paused : "") + "\n\n"
                ;
        }

        public string TransformTooltip()
        {
            Position pos = ConsoleAdventure.MouseWorld;
            return
                $"│ {TextAssets.FloorTooltip + Transform.GetName(pos, 0, ConsoleAdventure.curDeep)}\n" +
                $"│ {TextAssets.BlockTooltip + Transform.GetName(pos, 1, ConsoleAdventure.curDeep)}\n" +
                $"│ {TextAssets.LootTooltip + Transform.GetName(pos, 2, ConsoleAdventure.curDeep)}\n" +
                $"│ {TextAssets.EntityTooltip + Transform.GetName(pos, 3, ConsoleAdventure.curDeep)}\n\n"
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
                $"{world.GetLocalPlayer().inventory.GetInfo()}\n" +
                $"{Loger.GetLogs()}"
                ;
        }
    }
}
