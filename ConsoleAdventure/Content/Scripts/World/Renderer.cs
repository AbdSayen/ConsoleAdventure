using System;
using System.Collections.Generic;
namespace ConsoleAdventure.World
{
    internal class Renderer
    {
        List<List<List<Field>>> fields;
        public Renderer(List<List<List<Field>>> fields) {
            this.fields = fields;
        }

        public string Render(Transform observer)
        {
            string output = "";
            for (int y = observer.y - Console.WindowHeight / 3; y < observer.y + Console.WindowHeight / 3; y++)
            {
                if (y <= fields[World.FloorLayerId].Count - 1 && y >= 0)
                {
                    for (int x = observer.x - Console.WindowWidth / 6; x < observer.x + Console.WindowWidth / 6; x++)
                    {
                        if (x <= fields[World.FloorLayerId][y].Count - 1 && x >= 0)
                        {
                            if (fields[World.MobsLayerId][y][x] != null)
                            {
                                output += fields[World.MobsLayerId][y][x].GetSymbol();
                            }
                            else if (fields[World.BlocksLayerId][y][x] != null)
                            {
                                output += fields[World.BlocksLayerId][y][x].GetSymbol();
                            }
                        }
                        else
                        {
                            output += " #";
                        }
                    }
                    output += "\n";
                }
                else
                {
                    string outputX = "";
                    for (int x = 0; x < Console.WindowWidth / 3; x++)
                    {
                        outputX += " #";
                    }
                    output += outputX + "\n";
                }
            }
            return output;
        }
    }
}
