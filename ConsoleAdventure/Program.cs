using System;

namespace ConsoleAdventure
{
    public class Program
    {
        public static readonly string savePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ConsoleAdventure\\";

        public static void Main()
        {
            var game = new ConsoleAdventure();
            game.Run();
        }
    }
}
