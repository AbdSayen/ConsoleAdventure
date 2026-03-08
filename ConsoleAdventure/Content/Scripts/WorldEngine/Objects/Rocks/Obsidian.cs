using ConsoleAdventure.Content.Scripts.MaterialTypes;
using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using ConsoleAdventure.Content.Scripts.MaterialLogic;

namespace ConsoleAdventure.WorldEngine
{
    public class Obsidian : Transform
    {
        static string[] symbolsMap = new string[]
        {
            "/ ",
            " /",
            "\\ ",
            " \\",
            "Ϟ ",
            " Ϟ",
        };

        public Obsidian(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.obsidian;
            Initialize();
        }

        public override void SetStaticData()
        {
            Func<Transform, Item, SmartColor> color = (i, t) =>
            {
                return new(25, 0, 84);
            };
            
            var symbol = StoneType.GetModifySymbol("Ϟ", "ն", "¬", "նն", new[] { " /", " ¬", " Ϟ" });
            CreateMaterial(new StoneType(), color, symbol);

            IsObstacle[type] = true;
            Hardness[type] = 1.1f;
            BurnType[type] = 2;
        }

        public override void Collapse() => DropItem(new StoneItem(), 1, MaterialSystem.GetMaterial(GetType().Name).Type);

        public override string GetSymbol() => GetVariation(symbolsMap);

        public override Color GetColor() => new Color(0, 0, 0);

        public override Color? GetBGColor() => new Color(25, 0, 84);
    }
}
