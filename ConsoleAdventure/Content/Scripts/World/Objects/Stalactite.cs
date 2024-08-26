using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;

namespace ConsoleAdventure.WorldEngine
{
    [Serializable]
    public class Stalactite : Transform
    {
        static string[] symbolsMap = new string[] //VΛvᴧᵛᶺ
        {
            "VV",
            "ΛΛ",

            "vv",
            "ᴧᴧ",

            "ᵛᵛ",
            "ᶺᶺ",

            " V",
            "Λ ",

            " v",
            "ᴧ ",

            " ᵛ",
            "ᶺ ",

            "Vv",
            "Λᴧ",

            "vᵛ",
            "ᴧᶺ",

            "ᵛV",
            "ᶺΛ",

            "Vᴧ",
            "Λv",

            "vᶺ",
            "ᴧᵛ",

            "ᵛΛ",
            "ᶺV",
        };

        public Stalactite(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            if (worldLayer == -1) this.worldLayer = World.BlocksLayerId;
            else this.worldLayer = (byte)worldLayer;

            type = (int)RenderFieldType.stalactite;
            isObstacle = false;
            hardness = 4f;

            AddTypeToMap<Stalactite>(type);

            Initialize();
        }

        public override void Collapse()
        {
            new Loot(position, w, new List<Stack>() { new Stack(new StoneItem(), 3) });
        }

        public override string GetSymbol()
        {
            return symbolsMap[Utils.HashNoise(position.x, position.y, symbolsMap.Length - 1)];
        }

        public override Color GetColor()
        {
            return Color.Gray;
        }
    }
}