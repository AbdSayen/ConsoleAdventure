using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Content.Scripts.MaterialLogic;
using ConsoleAdventure.Content.Scripts.MaterialTypes;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine
{
    public class AppleTree : Tree
    {
        static string[] symbolsMap = new string[]
        {
            "-O", "O-", "Oʹ",
            ">O", "O~", "~O",
            "()", "o-", "~o"
        };

        public AppleTree(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.tree;
            Initialize();
        }

        public override void SetStaticData()
        {
            CreateMaterial(new WoodType(), "AppleWood", (i, t) => new(ColorAssets.woodenColor));

            SetDefaultStaticData();
            Crowns[type] = new string[,]
            {
                { "  ", "  ", "¾¾", "¾¾", "¾¾", "  ", "  " },
                { "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "  " },
                { "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾" },
                { "¾¾", "¾¾", "¾¾", "  ", "¾¾", "¾¾", "¾¾" },
                { "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾" },
                { "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "  " },
                { "  ", "¾¾", "¾¾", "¾¾", "¾¾", "  ", "  ",},
            };

            CrownColors[type] = new Color(13, 152, 20) * 0.5f;

            LogTypes[type] = typeof(Log);
            LogMaterials[type] = MaterialSystem.GetMaterial("AppleWood").Type;
            TrunkLogCounts[type] = 2..3;
            BranchesLogMaxCounts[type] = 2;

            FruitTypes[type] = typeof(Apple);
        }

        public override string GetSymbol() => GetVariation(symbolsMap);

        public override Color GetColor() => new(94, 61, 38);
    }
}
