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
                (ConsoleAdventure.isPause ? TextAssets.Paused : "") + "\n\n"
                ;
        }

        public string TransformTooltip()
        {
            Position pos = ConsoleAdventure.MouseWorld;
            return
                $"│ {Localization.GetTranslation("UI", "Floor") + Transform.GetName(pos, 0)}\n" +
                $"│ {Localization.GetTranslation("UI", "Block") + Transform.GetName(pos, 1)}\n" +
                $"│ {Localization.GetTranslation("UI", "Loot") + Transform.GetName(pos, 2)}\n" +
                $"│ {Localization.GetTranslation("UI", "Entity") + Transform.GetName(pos, 3)}\n\n"
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
