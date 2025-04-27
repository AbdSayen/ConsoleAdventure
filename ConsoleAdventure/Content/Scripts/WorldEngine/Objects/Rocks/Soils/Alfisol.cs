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

        public Alfisol(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            this.worldLayer = World.BlocksLayerId;

            type = (byte)VanillaTransforms.alfisol;

            isObstacle = true;
            hardness = 0.5f;

            AddTypeToMap<Alfisol>(type);
            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack> { new Stack(new AlfisolItem(), 1) });
        }

        public override string GetSymbol()
        {
            return symbolsMap[Utils.HashNoise(position.x, position.y, 8)];
        }

        public override Color GetColor()
        {
            return new Color(134, 107, 81);
        }

        public override Color? GetBGColor()
        {
            return new Color(54, 43, 32);
        }
    }
}
