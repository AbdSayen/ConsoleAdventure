using CaModLoaderAPI;
using ConsoleAdventure;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    public class BlackSoilFloor : Transform
    {
        static string[] symbolsMap = new string[]
        {
            " ~",
            " ≈",
            " .",
            " ,",
            " `",
        };

        public BlackSoilFloor(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            this.worldLayer = World.FloorLayerId;

            type = (byte)VanillaTransforms.blackSoilFloor;

            isObstacle = true;

            AddTypeToMap<BlackSoilFloor>(type);
            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack> { new Stack(new BlackSoilFloorItem(), 1) });
        }

        public override string GetSymbol()
        {
            return symbolsMap[Utils.HashNoise(position.x, position.y, 5)];
        }

        public override Color GetColor()
        {
            return new Color(20, 20, 20);
        }
    }
}
