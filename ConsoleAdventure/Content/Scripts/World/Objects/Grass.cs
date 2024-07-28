using Microsoft.Xna.Framework;
using System;
using System.CodeDom;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Grass : Transform
    {

        static string[] symbolsMap = new string[] //√♠♣ɾɿɼϔϒ☼"'rϓՐ
        {
            //"√\"",
            "'√",
            "√√",
            //"Ր√",
            "√r",
            "♣♣",
            "√♣",
            //"ϔr",
            "ϔϔ",
            "ϒ ",
            " ϒ",
            "\"\"",
            //"''",
            //"ɿ,",
            //",ɼ",
            ",'",
            "',",
            //"ɾr",
            " r"
        };

        static float darkDegree = 2.5f;

        static Color[] colorsMap = new Color[]
        {
            new(106, 255, 0) / darkDegree,
            new(76, 182, 0) / darkDegree,
            new(52, 124, 0) / darkDegree,
            new(17, 255, 0) / darkDegree,
            new(13, 191, 0) / darkDegree,
            new(8, 120, 0) / darkDegree,
            new(0, 255, 72) / darkDegree,
            new(0, 200, 57) / darkDegree,
            new(0, 112, 32) / darkDegree
        };

        int Sindex; //Symbol
        int Cindex; //Color

        public Grass(Position position, int worldLayer = -1) : base(position)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = worldLayer;

            type = (int)RenderFieldType.grass;
            isObstacle = false;

            AddTypeToMap<Grass>(type);

            Sindex = ConsoleAdventure.rand.Next(0, symbolsMap.Length);
            Cindex = ConsoleAdventure.rand.Next(0, colorsMap.Length);

            Initialize();
        }

        public override string GetSymbol()
        {
            return symbolsMap[Sindex];
        }

        public override Color GetColor()
        {
            return colorsMap[Cindex];
        }
    }
}
