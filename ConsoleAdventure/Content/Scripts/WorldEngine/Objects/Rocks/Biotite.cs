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
    public class Biotite : Transform
    {
        public Biotite(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.biotite;
            Initialize();
        }

        public override void SetStaticData()
        {
            Func<Transform, Item, SmartColor> color = (i, t) => new(35, 35, 35);

            var symbol = StoneType.GetModifySymbol("≡", "Ξ", "-", "ΞΞ", new[] { " -", " =" });
            CreateMaterial(new StoneType(), color, symbol);

            IsObstacle[type] = true;
            Hardness[type] = 0.5f;
        }

        public override void Collapse() => DropItem(new StoneItem(), 1, MaterialSystem.GetMaterial(GetType().Name).Type);

        public override string GetSymbol() => "≡≡";

        public override Color GetColor() => new Color(45, 45, 45);

        public override Color? GetBGColor() => new Color(15, 15, 15);
    }
}
