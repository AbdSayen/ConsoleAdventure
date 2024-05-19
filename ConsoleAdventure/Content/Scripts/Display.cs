using ConsoleAdventure.Settings;
using Microsoft.Xna.Framework;

namespace ConsoleAdventure
{
    internal class Display
    {
        public string Show(WorldEngine.World world)
        {
            return 
                $"{Docs.GetInfo()}\n" +
                $"{world.time.GetTime()}\n" +
                $"X:{world.players[0].position.x} Y:{world.players[0].position.y}\n" +
                $"Structure: {world.fields[WorldEngine.World.BlocksLayerId][world.players[0].position.y][world.players[0].position.x].structureName}\n\n" +
                $"{world.Render()}" +
                $"{Loger.GetLogs()}"
                ;
        }
    }
}
