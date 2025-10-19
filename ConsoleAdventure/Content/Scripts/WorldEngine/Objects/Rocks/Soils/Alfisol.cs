using CaModLoaderAPI;
using ConsoleAdventure;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class Alfisol : Transform
    {
        static string[] symbolsMap = new string[]
        {
            "~~",
            "~≈",
            "≈~",
            "≈≈",
            "~ ",
            " ~",
            " ≈",
            "≈ "
        };

        public Alfisol(Position position, int w) : base(position, (byte)w)
        {
            type = (byte)VanillaTransforms.alfisol;
            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
            Hardness[type] = 0.5f;
        }
        public override void Collapse() => DropItem(new AlfisolItem());

        public override string GetSymbol() => GetVariation(symbolsMap);

        public override Color GetColor() => new Color(134, 107, 81);

        public override Color? GetBGColor() => new Color(54, 43, 32);
    }
}
