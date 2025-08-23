using ConsoleAdventure.Content.Scripts.IO;
using System;
using System.Text;

namespace ConsoleAdventure
{
    public class Program
    {
        public static readonly string savePath = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\ConsoleAdventure\\";
        public static readonly string pcIdPath = savePath + "pc.id";

        public static ConsoleAdventure game;

        [STAThreadAttribute]
        public static void Main()
        {
            game = new ConsoleAdventure();

            try
            {
                game.Run();
            }
            catch (Exception ex)
            {
                ConsoleAdventure.logger.AddException(ex);

                try
                {
                    if (ConsoleAdventure.world != null)
                    {
                        StringBuilder name = new();
                        name.Append($"({ConsoleAdventure.world.name})");
                        int count = ConsoleAdventure.rand.Next(10, 16);

                        for (int i = 0; i < count; i++)
                        {
                            string symbol = "";
                            int type = ConsoleAdventure.rand.Next(0, 3);
                            Encoding encoding = Encoding.ASCII;

                            if (type == 0)
                                symbol = encoding.GetString(new byte[1] { (byte)ConsoleAdventure.rand.Next(48, 58) });

                            else if (type == 1)
                                symbol = encoding.GetString(new byte[1] { (byte)ConsoleAdventure.rand.Next(65, 91) });

                            else if (type == 2)
                                symbol = encoding.GetString(new byte[1] { (byte)ConsoleAdventure.rand.Next(97, 123) });

                            name.Append(symbol);
                        }

                        //WorldIO.Save(name.ToString());
                    }
                }

                catch (Exception ex1)
                {
                    ConsoleAdventure.logger.AddException(ex1);
                    throw;
                }

                throw;
            }
        }
    }
}
