using ConsoleAdventure.Content.Scripts;
using ConsoleAdventure.Content.Scripts.MaterialLogic;
using ConsoleAdventure.Content.Scripts.MaterialTypes;
using ConsoleAdventure.WorldEngine;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ConsoleAdventure.WorldEngine
{
    public class Granite : Transform
    {
        public Granite(Position position, int w) : base(position, (byte)w)
        {
            type = (int)VanillaTransforms.granite;
            Initialize();
        }

        public override void SetStaticData()
        {
            Func<Transform, Item, SmartColor> color = (i, t) =>
            {
                return new(40, 40, 40);
            };

            var symbol = StoneType.GetModifySymbol("#", "∫", "~", "∫∫", new[] { " .", " ~", " ," });
            CreateMaterial(new StoneType(), color, symbol);
            IsObstacle[type] = true;
            Hardness[type] = 2f;
            BurnType[type] = 2;
        }

        public override void Collapse() => DropItem(new StoneItem(), material: 0);

        public override string GetSymbol() => "##";

        public override Color GetColor() => new Color(10, 10, 10);

        public override Color? GetBGColor() => new Color(75, 75, 75);
    }
}
