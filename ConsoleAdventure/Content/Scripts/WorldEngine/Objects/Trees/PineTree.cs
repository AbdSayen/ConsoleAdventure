using ConsoleAdventure.Content.Scripts.IO;
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

        public PineTree(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.pineTree;
            AddTypeToMap();
            Initialize();
        }

        public override void SetStaticData()
        {
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
            FruitTypes[type] = typeof(Apple);
        }

        public override string GetSymbol()
        {
            return symbolsMap[Utils.HashNoise(position.x, position.y, symbolsMap.Length - 1)];
        }

        public override Color GetColor()
        {
            return new(88, 50, 22);
        }
    }
}
