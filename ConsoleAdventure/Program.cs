using ConsoleAdventure.Settings;
using System;

namespace ConsoleAdventure
{
    internal class Program
    {
        static void Main(string[] args)
        {
            World.World world = new World.World();
            string lastKey;

            while (true)
            {
                Console.Write($"{Docs.GetInfo()}\n\n{world.Render()}");
                lastKey = Console.ReadKey().ToString();
                Console.Clear();
            }
        }
    }
}
