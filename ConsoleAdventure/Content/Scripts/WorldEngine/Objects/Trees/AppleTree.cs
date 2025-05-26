using ConsoleAdventure.Content.Scripts.IO;
using Microsoft.Xna.Framework;
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

        public AppleTree(Position position, int w, int worldLayer = -1) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.tree;
            AddTypeToMap<AppleTree>(type);
            Initialize();
        }

        public override object SetStaticData()
        {
            Tags tags = new Tags();

            tags["Crown"] = new string[,]
            {
                { "  ", "  ", "¾¾", "¾¾", "¾¾", "  ", "  " },
                { "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "  " },
                { "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾" },
                { "¾¾", "¾¾", "¾¾", "  ", "¾¾", "¾¾", "¾¾" },
                { "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾" },
                { "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "¾¾", "  " },
                { "  ", "¾¾", "¾¾", "¾¾", "¾¾", "  ", "  ",},
            };

            tags["CrownColor"] = new Color(13, 152, 20) * 0.5f;

            tags["LogType"] = typeof(Log);
            tags["FruitType"] = typeof(Apple);

            return tags;
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
