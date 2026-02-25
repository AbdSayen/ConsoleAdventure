using ConsoleAdventure.Content.Scripts.IO;
using ConsoleAdventure.Content.Scripts.MaterialLogic;
using ConsoleAdventure.Content.Scripts.MaterialTypes;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine
{
    public class PineTree : Tree
    {
        static string[] symbolsMap = new string[]
        {
            "-O", "O<", "Oʹ",
            ">O", "O~", "`O",
            "()", "o^", ">o"
        };

        public PineTree(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.pineTree;
            Initialize();
        }

        public override void SetStaticData()
        {
            CreateMaterial(new WoodType(), "PineWood", (i, t) => new(224, 175, 133));

            SetDefaultStaticData();
            Crowns[type] = new string[,]
            {
                { "  ", "  ", "¾¾", "  ", "¾¾", "  ", "  " },
                { "  ", "¾¾", "¾¾", " ¾", "¾¾", "¾¾", "¾ " },
                { " ¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "  " },
                { "  ", "¾¾", "¾¾", "  ", "¾¾", "¾¾", "¾ " },
                { "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾" },
                { " ¾", "¾¾", " ¾", "¾¾", "¾¾", "¾¾", "  " },
                { "  ", "  ", "  ", "¾¾", " ¾", " ¾", "  ",},
            };

            CrownColors[type] = new Color(42, 113, 26) * 0.7f;

            LogTypes[type] = typeof(Log);
            LogMaterials[type] = MaterialSystem.GetMaterial("PineWood").Type;
            TrunkLogCounts[type] = 4..6;
            BranchesLogMaxCounts[type] = 1;

            //FruitTypes[type] = typeof(Apple);
        }

        public override string GetSymbol() => GetVariation(symbolsMap);

        public override Color GetColor() => new(88, 50, 22);
    }
}
