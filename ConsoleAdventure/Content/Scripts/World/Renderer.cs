﻿using System.Collections.Generic;
using System.Text;

namespace ConsoleAdventure.WorldEngine
{
    public class Renderer
    {
        List<List<List<Field>>> fields;
        private int viewDistanceY = 30;
        private int viewDistanceX = 60;
        public Renderer(List<List<List<Field>>> fields) {
            this.fields = fields;
        }

        public string Render(Transform observer, int layer)
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
                            if (fields[World.MobsLayerId][y][x] != null && layer == World.MobsLayerId)
                            {
                                output += fields[World.MobsLayerId][y][x].GetSymbol();
                            }
                            else if (fields[World.ItemsLayerId][y][x] != null && layer == World.ItemsLayerId)
                            {
                                output += fields[World.ItemsLayerId][y][x].GetSymbol();
                            }
                            else if (fields[World.BlocksLayerId][y][x] != null && layer == World.BlocksLayerId)
                            {
                                output += fields[World.BlocksLayerId][y][x].GetSymbol();
                            }
                            else if (fields[World.FloorLayerId][y][x] != null && layer == World.FloorLayerId)
                            {
                                output += fields[World.FloorLayerId][y][x].GetSymbol();
                            }
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
            StringBuilder output = new StringBuilder();
            for (int y = 0; y < fields[World.FloorLayerId].Count; y++)
            {
                for (int x = 0; x < fields[World.FloorLayerId][y].Count; x++)
                {
                    output.Append(fields[World.BlocksLayerId][y][x].GetSymbol());
                }
                output.AppendLine();
            }
            return output.ToString();
        }
    }
}
