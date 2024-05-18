using ConsoleAdventure.Settings;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    internal class Renderer
    {
        List<List<List<Field>>> fields;
        private int viewDistanceY = 20;
        private int viewDistanceX = 40;
        public Renderer(List<List<List<Field>>> fields) {
            this.fields = fields;
        }

        public string Render(Transform observer)
        {
            string output = "";
            for (int y = observer.position.y - viewDistanceY / 2; y < observer.position.y + viewDistanceY / 2; y++)
            {
                if (y <= fields[World.FloorLayerId].Count - 1 && y >= 0)
                {
                    for (int x = observer.position.x - viewDistanceX / 2; x < observer.position.x + viewDistanceX / 2; x++)
                    {
                        if (x <= fields[World.FloorLayerId][y].Count - 1 && x >= 0)
                        {
                            output += fields[World.BlocksLayerId][y][x].GetSymbol();
                        }
                        else
                        {
                            output += " `";
                        }
                    }
                    output += "\n";
                }
                else
                {
                    string outputX = "";
                    for (int x = 0; x < viewDistanceX; x++)
                    {
                        outputX += " `";
                    }
                    output += outputX + "\n";
                }
            }
            return output;
        }

        public string PrimitiveRender()
        {
            string output = string.Empty;
            for (int y = 0; y < fields[World.FloorLayerId].Count; y++)
            {
                for (int x = 0; x < fields[World.FloorLayerId][y].Count; x++)
                {
                    output += fields[World.FloorLayerId][y][x].GetSymbol();
                }
                output += "\n";
                Loger.AddLog("Successful");
            }
            return output;
        }
    }
}
