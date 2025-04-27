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

        public BlackSoil(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            this.worldLayer = World.BlocksLayerId;

            type = (byte)VanillaTransforms.blackSoil;

            isObstacle = true;
            hardness = 0.5f;

            AddTypeToMap<BlackSoil>(type);
            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack> { new Stack(new BlackSoilItem(), 1) });
        }

        public override string GetSymbol()
        {
            return symbolsMap[Utils.HashNoise(position.x, position.y, 8)];
        }

        public override Color GetColor()
        {
            return new Color(30, 30, 30);
        }

        public override Color? GetBGColor()
        {
            return new Color(10, 10, 10);
        }
    }
}
