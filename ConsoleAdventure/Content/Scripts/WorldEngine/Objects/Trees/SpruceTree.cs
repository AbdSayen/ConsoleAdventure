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

        public SpruceTree(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.spruceTree;
            AddTypeToMap<SpruceTree>(type);
            Initialize();
        }

        public override void SetStaticData()
        {
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

        public override string GetSymbol()
        {
            return symbolsMap[Utils.HashNoise(position.x, position.y, symbolsMap.Length - 1)];
        }

        public override Color GetColor()
        {
            return new(94, 61, 38);
        }
    }
}
