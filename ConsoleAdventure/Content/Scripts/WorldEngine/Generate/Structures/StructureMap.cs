using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.Generate.Structures
{
    public static class StructureMap
    {
        public static List<Rectangle> Structures { get; private set; } = new List<Rectangle>(); 
    }

    public struct StructureUnit
    {
        public int x;
        public int y;
        public int w;

        public int width;
        public int height;

        public string name;

        public StructureUnit(int x, int y, int w, int width, int height, string name)
        {
            this.x = x;
            this.y = y;
            this.w = w;
            this.width = width;
            this.height = height;
            this.name = name;
        }

        public bool IsInside(int x, int y, int w)
        {
            return (w == this.w && x > this.x && y > this.y && x < this.x + width && y < this.y + height);
        }
    }
}
