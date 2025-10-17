using ConsoleAdventure.Content.Scripts.IO;
using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine
{
    public class SpruceTree : Tree
    {
        static string[] symbolsMap = new string[]
        {
            "-O", "O-", "Oʹ",
            ">O", "O~", "~O",
            "()", "o-", "~o"
        };

        public SpruceTree(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.spruceTree;
            Initialize();
        }

        public override void SetStaticData()
        {
            SetDefaultStaticData();
            Crowns[type] = new string[,]
            {
                //{ "  ", " ¾", "¾ ", "  ", "  ", "  ", "  " },
                { "  ", "  ", "¾¾", "¾ ", " ¾", "  ", "  " },
                { "¾¾", "¾ ", "¾¾", "¾¾", "¾¾", "  ", "¾¾" },
                { " ¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾ " },
                { "¾¾", "¾¾", "¾¾", "  ", "¾¾", "¾¾", "  " },
                { "  ", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾" },
                { " ¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", " ¾" },
                { "¾¾", "  ", "¾¾", "¾¾", "  ", "¾¾", "  " },
                //{ "  ", "  ", "¾¾", "  ", "  ", " ¾", "  " },
            };

            CrownColors[type] = new Color(17, 54, 27);

            LogTypes[type] = typeof(Log);
            FruitTypes[type] = typeof(Apple);
        }

        public override string GetSymbol() => GetVariation(symbolsMap);

        public override Color GetColor() => new(94, 61, 38);
    }
}
