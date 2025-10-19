using CaModLoaderAPI;
using ConsoleAdventure;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class BlackSoil : Transform
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

        public BlackSoil(Position position, int w) : base(position, (byte)w)
        {
            type = (byte)VanillaTransforms.blackSoil;
            Initialize();
        }

        public override void SetStaticData()
        {
            IsObstacle[type] = true;
            Hardness[type] = 0.5f;
        }
        public override void Collapse() => DropItem(new BlackSoilItem());

        public override string GetSymbol() => GetVariation(symbolsMap);

        public override Color GetColor() => new Color(30, 30, 30);

        public override Color? GetBGColor() => new Color(10, 10, 10);
    }
}
