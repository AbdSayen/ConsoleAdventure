using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.MaterialLogic;
using ConsoleAdventure.Content.Scripts.MaterialTypes;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAdventure.WorldEngine
{
    public class Granulite : Transform
    {
        public Granulite(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.granulite;
            Initialize();
        }

        public override void SetStaticData()
        {
            Func<Transform, Item, SmartColor> color = (i, t) => new(110, 110, 78); //(100, 100, 73)

            var symbol = StoneType.GetModifySymbol("≤", "Ո", "<", "ՈՈ", new[] { " -", " ~", " <", });
            CreateMaterial(new StoneType(), color, symbol);

            IsObstacle[type] = true;
            Hardness[type] = 2f;
            BurnType[type] = 2;
        }

        public override void Collapse() => DropItem(new StoneItem(), material: 1);

        public override string GetSymbol() => "≤≤";

        public override Color GetColor() => new Color(64, 19, 19);

        public override Color? GetBGColor() => new Color(100, 100, 73);
    }
}
