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
            //"Ր√",
            //"ϔr",
            //"''",
            //"ɿ,",
            //",ɼ",
            //"ɾr",
            "'√",
            "√√",  
            "√r",
            "♣♣",
            "√♣",         
            "ϔϔ",
            "ϒ ",
            " ϒ",
            "\"\"",
            ",'",
            "',",
            " r"
        };

        static float darkDegree = 1.6f;

        static Color[] colorsMap = new Color[]
        {
            new(106, 255, 0),
            new(76, 182, 0),
            new(52, 124, 0),
            new(17, 255, 0),
            new(13, 191, 0),
            new(8, 120, 0),
            new(0, 255, 72),
            new(0, 200, 57),
            new(0, 112, 32)
        };

        public Grass(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)RenderFieldType.grass;
            isObstacle = false;
            hardness = 0.1f;

            AddTypeToMap<Grass>(type);
            Initialize();
        }

        public override string GetSymbol()
        {
            return symbolsMap[Utils.HashNoise(position.x, position.y, 12)];
        }

        public override Color GetColor()
        {
            return colorsMap[Utils.HashNoise(position.x, position.y, 9)];
        }
    }
}
