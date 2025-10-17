using Microsoft.Xna.Framework;
using System;
using System.CodeDom;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class ObsidianFloor : Transform
    {
        static string[] symbolsMap = new string[]
        {
            " /",
            " ¬",
            " Ϟ",
        };

        public ObsidianFloor(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            this.position = position;
            if (worldLayer == -1) this.worldLayer = World.FloorLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)VanillaTransforms.obsidianFloor;

            AddTypeToMap();

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack> { new Stack(new ObsidianFloorItem(), 1) });
        }

        public override string GetSymbol()
        {
            return symbolsMap[Utils.HashNoise(position.x, position.y, 3)];
        }

        public override Color GetColor()
        {
            return new Color(25, 0, 84);
        }
    }
}