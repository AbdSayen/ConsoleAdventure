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
    public class Basalt : Transform
    {
        public Basalt(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.basalt;
            Initialize();
        }

        public override void SetStaticData()
        {
            Func<Transform, Item, SmartColor> color = (t, i) =>
            {
                if (t?.type == (byte)VanillaTransforms.floor)
                    return new(35, 35, 35);

                return new(45, 45, 45);
            };

            var symbol = StoneType.GetModifySymbol("‡", "Ɨ", "“", "ƗƗ", new[] { " -", " *", " “" });
            CreateMaterial(new StoneType(), color, symbol);

            IsObstacle[type] = true;
            Hardness[type] = 1.8f;
            BurnType[type] = 2;
        }

        public override void Collapse() => DropItem(new StoneItem(), 1, MaterialSystem.GetMaterial(GetType().Name).Type);

        public override string GetSymbol() => "‡‡";

        public override Color GetColor() => new Color(10, 10, 10);

        public override Color? GetBGColor() => new Color(45, 45, 45);
    }
}
