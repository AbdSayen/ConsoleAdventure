using Microsoft.Xna.Framework;
using System;
using System.CodeDom;

namespace ConsoleAdventure.WorldEngine
{
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

        public Grass(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.grass;
            Initialize();
        }

        public override void SetStaticData()
        {
            Hardness[type] = 0.1f;
            BurnType[type] = 1;
        }

        public override string GetSymbol() => GetVariation(symbolsMap);

        public override Color GetColor() => GetVariation(colorsMap);
    }
}
